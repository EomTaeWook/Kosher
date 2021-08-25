using ValetonUtils.Framework;
using System.Collections.Generic;

namespace ValetonUtils.ObjectPool
{
    public class ObejctPool<T> :Singleton<ObejctPool<T>> where T : new()
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
            for(int i=0;i<activeObjects.Count; ++i)
            {
                if(activeObjects[i].GetHashCode() == item.GetHashCode())
                {
                    activeObjects.Remove(activeObjects[i]);
                    break;
                }
            }
            objectPools.Push(item);
        }
        public void Clear()
        {
            activeObjects.Clear();
            objectPools.Clear();
        }
    }
}
