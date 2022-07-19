using System.Collections.Generic;
using UnityEngine;

namespace KosherUnity
{
    public class KosherUnityObjectPool : SingletonWithMonoBehaviour<KosherUnityObjectPool>
    {
        private Dictionary<string, Stack<GameObject>> objectPools = new Dictionary<string, Stack<GameObject>>();
        private HashSet<GameObject> activeObjects = new HashSet<GameObject>();
        public static T CallLocation<T>(Component component) where T : Component
        {
            return CallLocation<T>(component.gameObject);
        }
        public static T CallLocation<T>(GameObject prefab) where T : Component
        {
            return CallLocation<T>(prefab, null);
        }
        public static T CallLocation<T>(Component component, Transform parent) where T : Component
        {
            return CallLocation<T>(component.gameObject, parent);
        }
        public static T CallLocation<T>(GameObject prefab, Transform parent) where T : Component
        {
            var component = KosherUnityObjectPool.Instance.Pop<T>(prefab);
            component.gameObject.transform.SetParent(parent, false);
            component.gameObject.gameObject.SetActive(true);
            return component;
        }
        public KosherUnityObjectPool()
        {
        }

        public GameObject Pop(GameObject item)
        {
            GameObject go;
            var typeName = item.name;
            if (objectPools.ContainsKey(typeName) == false)
            {
                objectPools.Add(typeName, new Stack<GameObject>());
            }
            if (objectPools[typeName].Count > 0)
            {
                go = objectPools[typeName].Pop();
            }
            else
            {
                go = GameObject.Instantiate(item);
            }
            activeObjects.Add(go);
            return go;
        }
        public T Pop<T>(GameObject item) where T : Component
        {
            var go = Pop(item);
            return go.GetComponent<T>();
        }
        public void Push(Component item)
        {
            Push(item.gameObject);
        }
        public void Push(GameObject item)
        {
            var typeName = item.name;
            if (activeObjects.Contains(item) == true)
            {
                activeObjects.Remove(item);
            }
            if (CheckAlreadyPool(item) == true)
            {
                return;
            }
            objectPools[typeName].Push(item);
        }
        public void Clear()
        {
            foreach (var kv in objectPools)
            {
                while (objectPools[kv.Key].Count > 0)
                {
                    var item = objectPools[kv.Key].Pop();
                    Destroy(item);
                }
            }
        }
        private bool CheckAlreadyPool(GameObject item)
        {
            if (objectPools.ContainsKey(item.name) == false)
            {
                objectPools.Add(item.name, new Stack<GameObject>());
            }
            return objectPools[item.name].Contains(item);
        }
    }
}
