using System.Collections.Generic;
using UnityEngine;

namespace KosherUnity
{
    public class KosherUnityObjectPool : SingletonWithMonoBehaviour<KosherUnityObjectPool>
    {
        private Dictionary<System.Type, Stack<Component>> objectPools = new Dictionary<System.Type, Stack<Component>>();
        private HashSet<Component> activeObjects = new HashSet<Component>();
        public static T CallLocation<T>(T prefab) where T : Component
        {
            return CallLocation(prefab, null);
        }
        public static T CallLocation<T>(GameObject go, Transform parent) where T : Component
        {
            return CallLocation(go.GetComponent<T>(), parent);
        }
        public static T CallLocation<T>(T prefab, Transform parent) where T : Component
        {
            var component = KosherUnityObjectPool.Instance.Pop<T>(prefab);
            component.gameObject.transform.SetParent(parent);
            component.gameObject.gameObject.SetActive(true);
            return component;
        }
        public KosherUnityObjectPool()
        {
        }

        public T Pop<T>(Component prefab) where T: Component
        {
            T obj;
            var type = typeof(T);
            if (objectPools.ContainsKey(type) == false)
            {
                objectPools.Add(type, new Stack<Component>());
            }
            if (objectPools[type].Count > 0)
            {
                var item = objectPools[type].Pop();
                obj = item.GetComponent<T>();
            }
            else
            {
                var go = GameObject.Instantiate(prefab);
                obj = go.GetComponent<T>();
            }
            activeObjects.Add(obj);
            return obj;
        }
        public void Push(Component item)
        {
            if(activeObjects.Contains(item) == true)
            {
                activeObjects.Remove(item);
            }

            if (CheckAlreadyPool(item) == true)
            {
                return;
            }
            objectPools[item.GetType()].Push(item);
        }
        private bool CheckAlreadyPool(Component item)
        {
            return objectPools[item.GetType()].Contains(item);
        }
    }
}
