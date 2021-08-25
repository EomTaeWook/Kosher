using UnityEngine;

namespace KosherUnity
{
    public static class KosherObjectPoolExtensions
    {
        public static void Recall(this GameObject gameObject)
        {
            gameObject.transform.SetParent(KosherUnityObjectPool.Instance.transform);
            gameObject.SetActive(false);
            KosherUnityObjectPool.Instance.Push(gameObject);
        }
        public static void Recall<T>(this Component component) where T : Object
        {
            component.transform.SetParent(KosherUnityObjectPool.Instance.transform);
            component.gameObject.SetActive(false);
            KosherUnityObjectPool.Instance.Push(component.gameObject);
        }
    }
}
