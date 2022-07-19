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
            component.gameObject.transform.SetParent(parent);
            component.gameObject.gameObject.SetActive(true);
            return component;
        }
        public KosherUnityObjectPool()
        {
        }

        public GameObject Pop(GameObject prefab)
        {
            GameObject go;
            var typeName = prefab.name;
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
                go = GameObject.Instantiate(prefab);
            }
            activeObjects.Add(go);
            return go;
        }
        public T Pop<T>(GameObject prefab) where T : Component
        {
            var go = Pop(prefab);
            return go.GetComponent<T>();
        }
        public void Push(Component item)
        {
            var typeName = item.gameObject.name;
            if (activeObjects.Contains(item.gameObject) == true)
            {
                activeObjects.Remove(item.gameObject);
            }
            if (CheckAlreadyPool(item.gameObject) == true)
            {
                return;
            }
            objectPools[typeName].Push(item.gameObject);
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
                return true;
            }
            return objectPools[item.name].Contains(item);
        }
    }
}
