using UnityEngine;

namespace KosherUnity
{
    public static class KosherObjectPoolExtensions
    {
        public static void Recycle<T>(this GameObject gameObject) where T : Component
        {
            gameObject.transform.SetParent(KosherUnityObjectPool.Instance.transform);
            gameObject.SetActive(false);
            KosherUnityObjectPool.Instance.Push(gameObject.GetComponent<T>());
        }
        public static void Recycle<T>(this Component component)
        {
            component.gameObject.transform.SetParent(KosherUnityObjectPool.Instance.transform);
            component.gameObject.SetActive(false);
            KosherUnityObjectPool.Instance.Push(component);
        }
    }
}
