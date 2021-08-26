using KosherUtils.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace KosherUnity
{
    public class KosherUnityResourceCache : Singleton<KosherUnityResourceCache>
    {
        private Dictionary<string, UnityEngine.Object> cacheDatas = new Dictionary<string, UnityEngine.Object>();

        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            return KosherUnityResourceCache.Instance.LoadResouce<T>(path);
        }
        public T LoadResouce<T>(string path) where T : UnityEngine.Object
        {
            if(cacheDatas.ContainsKey(path) == false)
            {
                cacheDatas.Add(path, Resources.Load<T>(path));
            }

            return (T)cacheDatas[path];
        }
        public void UnloadResource(string path)
        {
            if (cacheDatas.ContainsKey(path) == true)
            {
                Resources.UnloadAsset(cacheDatas[path]);
                cacheDatas.Remove(path);
            }
        }
    }
}
