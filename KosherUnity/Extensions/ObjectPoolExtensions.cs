using UnityEngine;

namespace KosherUnity
{
    public static class ObjectPoolExtensions
    {
        public static void Recycle(this GameObject gameObject)
        {
            gameObject.transform.SetParent(KosherUnityObjectPool.Instance.transform);
            gameObject.SetActive(false);
            KosherUnityObjectPool.Instance.Push(gameObject);
        }
        public static void Recycle<T>(this Component component) where T : Component
        {
            component.transform.SetParent(KosherUnityObjectPool.Instance.transform);
            component.gameObject.SetActive(false);
            KosherUnityObjectPool.Instance.Push(component.gameObject);
        }
    }
}
