using KosherUtils.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace KosherUnity
{
    public class KosherUnityResourceCache : Singleton<KosherUnityResourceCache>
    {
        private Dictionary<string, Object> cacheDatas = new Dictionary<string, Object>();

        public static T Load<T>(string path) where T : Object
        {
            return KosherUnityResourceCache.Instance.LoadResouce<T>(path);
        }
        public T LoadResouce<T>(string path) where T : Object
        {
            if(cacheDatas.ContainsKey(path) == false)
            {
                cacheDatas.Add(path, Resources.Load<T>(path));
            }

            return cacheDatas[path] as T;
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
