using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KosherUtils.Framework;
using UnityEngine.SceneManagement;

namespace KosherUnity
{
    public class KosherUnitySceneManager : Singleton<KosherUnitySceneManager>
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        public void LoadAddScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        public void LoadSceneAsync(string sceneName, Action onCallback)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            KosherUnityCoroutine.StartCoroutine(asyncOperation, (o) =>
            {
                onCallback?.Invoke();
            });
        }
    }
}
