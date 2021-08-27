using KosherUnity.Datas;
using KosherUtils.ObjectPool.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace KosherUnity
{

    public class KosherUnityObjectPool : SingletonWithMonoBehaviour<KosherUnityObjectPool>, IObjectPool<Component>
    {
        private List<ObjectPoolData> objectPool = new List<ObjectPoolData>();
        private List<ObjectPoolData> activeObjects = new List<ObjectPoolData>();

        public static T CallLocation<T>(T prefab) where T : Component
        {
            var component = KosherUnityObjectPool.Instance.Pop<T>(prefab.gameObject);
            component.gameObject.transform.SetParent(null);
            return component;
        }
        public static T CallLocation<T>(GameObject prefab, Transform parent) where T : Component
        {
            var component = KosherUnityObjectPool.Instance.Pop<T>(prefab);
            component.gameObject.transform.SetParent(parent);
            component.gameObject.gameObject.SetActive(true);
            return component;
        }

        public T Pop<T>(GameObject item) where T : Component
        {
            ObjectPoolData objectPoolData = null;
            for (int i = 0; i < objectPool.Count; ++i)
            {
                if(objectPool[i].Component.GetType() == typeof(T))
                {
                    objectPoolData = objectPool[i];
                    break;
                }
            }
            if(objectPoolData != null)
            {
                objectPool.Remove(objectPoolData);
            }
            if (objectPoolData == null)
            {
                var go = GameObject.Instantiate(item);
                var objectPoolItem = go.gameObject.GetComponent<ObjectPoolItem>();
                if (objectPoolItem == null)
                {
                    objectPoolItem = go.gameObject.AddComponent<ObjectPoolItem>();
                }
                objectPoolItem.Init(this);
                objectPoolData = new ObjectPoolData()
                {
                    Component = go.GetComponent<T>(),
                };
            }
            activeObjects.Add(objectPoolData);
            return objectPoolData.Component.gameObject.GetComponent<T>();
        }
        public void Push<T>(GameObject item) where T : Component
        {
            ObjectPoolData findObject = null;
            for (int i = 0; i < activeObjects.Count; ++i)
            {
                if (activeObjects[i].Component.gameObject == item)
                {
                    findObject = activeObjects[i];
                    break;
                }
            }
            if (findObject != null)
            {
                activeObjects.Remove(findObject);
                objectPool.Add(findObject);
            }
            else
            {
                if (CheckAlreadyPool(item) == true)
                {
                    return;
                }
                var objectPoolItem = item.gameObject.GetComponent<ObjectPoolItem>();
                if (objectPoolItem == null)
                {
                    objectPoolItem = item.gameObject.AddComponent<ObjectPoolItem>();
                }
                objectPoolItem.Init(this);
                findObject = new ObjectPoolData()
                {
                    Component = item.GetComponent<T>(),
                };
                objectPool.Add(findObject);
            }
        }
        public void Clear()
        {
            for (int i = 0; i < activeObjects.Count; ++i)
            {
                GameObject.Destroy(activeObjects[i].Component.gameObject);
            }
            activeObjects.Clear();
            for (int i = 0; i < objectPool.Count; ++i)
            {
                GameObject.Destroy(objectPool[i].Component.gameObject);
            }
            objectPool.Clear();
        }

        public void Push(Component item)
        {
            ObjectPoolData findObject = null;
            for (int i = 0; i < activeObjects.Count; ++i)
            {
                if (activeObjects[i].Component == item)
                {
                    findObject = activeObjects[i];
                    break;
                }
            }
            if (findObject != null)
            {
                activeObjects.Remove(findObject);
                objectPool.Add(findObject);
            }
            else
            {
                if(CheckAlreadyPool(item.gameObject) == true)
                {
                    return;
                }
                var objectPoolItem = item.gameObject.GetComponent<ObjectPoolItem>();
                if(objectPoolItem == null)
                {
                    objectPoolItem = item.gameObject.AddComponent<ObjectPoolItem>();
                }
                objectPoolItem.Init(this);
                findObject = new ObjectPoolData()
                {
                    Component = item,
                };
                objectPool.Add(findObject);
            }
        }
        private bool CheckAlreadyPool(GameObject item)
        {
            foreach(var poolItem in objectPool)
            {
                if(poolItem.Component.gameObject == item)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
