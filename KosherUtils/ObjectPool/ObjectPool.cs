using System.Collections.Generic;

namespace KosherUtils.ObjectPool
{
    public partial class ObjectPool<T> where T : class, new()
    {
        private Stack<T> objectPools = new Stack<T>();
        private HashSet<T> activeObjects = new HashSet<T>();
        public ObjectPool()
        {
        }
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
            if(activeObjects.Contains(item) == true)
            {
                activeObjects.Remove(item);
                return;
            }

            if (CheckAlreadyPool(item) == true)
            {
                return;
            }

            objectPools.Push(item);
        }
        public void Clear()
        {
            activeObjects.Clear();
            objectPools.Clear();
        }
        private bool CheckAlreadyPool(T item)
        {
            return objectPools.Contains(item);
        }
    }
}
