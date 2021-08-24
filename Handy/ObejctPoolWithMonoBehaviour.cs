using Handy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Handy
{
    public class HandyObjectPool : SingletonWithMonoBehaviour<HandyObjectPool>
    {
        private List<ObjectPoolData> objectPools = new List<ObjectPoolData>();
        private List<ObjectPoolData> activeObjects = new List<ObjectPoolData>();

        public static T Spawn<T>(T prefab) where T : Component
        {
            var component = HandyObjectPool.Instance.Pop<T>(prefab.gameObject);
            component.gameObject.transform.SetParent(null);
            return component;
        }
        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            var component = HandyObjectPool.Instance.Pop<T>(prefab.gameObject);
            component.gameObject.transform.SetParent(parent);
            component.gameObject.gameObject.SetActive(true);
            return component;
        }

        public T Pop<T>(GameObject item)
        {
            ObjectPoolData objectPoolData = null;
            for (int i = 0; i < objectPools.Count; ++i)
            {
                if (objectPools[i].Component.GetType() == typeof(T))
                {
                    objectPoolData = objectPools[i];
                    break;
                }
                else if(objectPools[i].Component == null)
                {
                    var go = GameObject.Instantiate(item);
                    objectPools[i].GameObjectData = go.gameObject;
                    objectPools[i].Component = gameObject.GetComponent<T>();
                    objectPoolData = objectPools[i];
                    break;
                }
            }
            if(objectPoolData != null)
            {
                objectPools.Remove(objectPoolData);
            }
            if (objectPoolData == null)
            {
                var go = GameObject.Instantiate(item);
                objectPoolData = new ObjectPoolData()
                {
                    GameObjectData = go.gameObject,
                    Component = go.GetComponent<T>()
                };
            }
            activeObjects.Add(objectPoolData);
            return objectPoolData.GameObjectData.GetComponent<T>();
        }
        public void Push(GameObject item)
        {
            ObjectPoolData findObject = null;
            for (int i = 0; i < activeObjects.Count; ++i)
            {
                if (activeObjects[i].GameObjectData == item)
                {
                    findObject = activeObjects[i];
                    break;
                }
            }
            if (findObject != null)
            {
                activeObjects.Remove(findObject);
                objectPools.Add(findObject);
            }
        }
        public void Clear()
        {
            for (int i = 0; i < activeObjects.Count; ++i)
            {
                GameObject.Destroy(activeObjects[i].GameObjectData);
            }
            activeObjects.Clear();

            for (int i = 0; i < objectPools.Count; ++i)
            {
                GameObject.Destroy(objectPools[i].GameObjectData);
            }
            objectPools.Clear();
        }
    }
}
