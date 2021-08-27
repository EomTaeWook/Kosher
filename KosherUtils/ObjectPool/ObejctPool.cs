using KosherUtils.Framework;
using KosherUtils.ObjectPool.Interface;
using System.Collections.Generic;

namespace KosherUtils.ObjectPool
{
    public class ObejctPool<T> :Singleton<ObejctPool<T>> where T : IObjectPoolItem, IObjectPool<T>, new()
    {
        private Stack<T> objectPools = new Stack<T>();
        private List<T> activeObjects = new List<T>();

        public void CreatePool(int maxSize)
        {
            objectPools.Clear();
            activeObjects.Clear();
            objectPools = new Stack<T>(maxSize);
            for (int i = 0; i < maxSize; ++i)
            {
                objectPools.Push(new T());
            }
        }

        public T Pop()
        {
            T item;
            if (objectPools.Count > 0)
            {
                item = objectPools.Pop();
            }
            else
            {
                item = new T();
            }
            activeObjects.Add(item);
            return item;
        }
        public void Push(T item)
        {
            T findObject = default;
            for(int i=0;i<activeObjects.Count; ++i)
            {
                if(activeObjects[i].GetHashCode() == item.GetHashCode())
                {
                    findObject = activeObjects[i];
                    break;
                }
            }

            if(findObject!=null)
            {
                activeObjects.Remove(findObject);
            }
            else
            {
                if(CheckAlreadyPool(item) == true)
                {
                    return;
                }
                findObject = item;
            }
            
            objectPools.Push(findObject);
        }
        public void Clear()
        {
            for(int i=0; i< activeObjects.Count; ++i)
            {
                activeObjects[i].Recycle();
            }
            objectPools.Clear();
        }
        private bool CheckAlreadyPool(T item)
        {
            return objectPools.Contains(item);
        }
    }
}
