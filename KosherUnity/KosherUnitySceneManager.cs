using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KosherUtils.Framework;
using UnityEngine.SceneManagement;
using KosherUnity.Coroutine;
using System.Collections;

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
            var handle = KosherUnityCoroutineManager.StartCoroutine(ProcessLoadScene(sceneName), () =>
            {
                onCallback?.Invoke();
            });

        }
        private IEnumerator ProcessLoadScene(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while(asyncOperation.isDone == false)
            {
                yield return null;
            }

            yield break;
        }
    }
}
