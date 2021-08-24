using System;
using System.Collections.Generic;

namespace Handy
{
    public class ObejctPool<T> : Singleton<ObejctPool<T>> where T : new()
    {
        private Stack<T> objectPools = new Stack<T>();
        private List<T> activeObjects = new List<T>();
        public void CreatePool(int maxSize)
        {
            activeObjects.Clear();
            objectPools.Clear();
            objectPools = new Stack<T>(maxSize);
            for(int i=0; i<maxSize; ++i)
            {
                objectPools.Push(new T());
            }
        }
        public T Pop()
        {
            T obj;
            if (objectPools.Count > 0)
            {
                obj = objectPools.Pop();
            }
            else
            {
                obj = new T();
            }
            activeObjects.Add(obj);
            return obj;
        }
        public void Push(T obj)
        {
            var find = false;
            for(int i=0; i<activeObjects.Count; ++i)
            {
                if(activeObjects[i].GetHashCode() == obj.GetHashCode())
                {
                    find = true;
                    break;
                }
            }
            if(find == true)
            {
                activeObjects.Remove(obj);
            }
            objectPools.Push(obj);
        }
        public void Clear()
        {
            activeObjects.Clear();
            objectPools.Clear();
        }
    }
}
