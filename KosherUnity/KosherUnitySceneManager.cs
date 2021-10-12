using KosherUnity.Coroutine;
using KosherUtils.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KosherUnity
{
    public class KosherUnitySceneManager : Singleton<KosherUnitySceneManager>
    {
        public string PreviousSceneName { get; private set; }

        public string CurrentSceneName { get; private set; }

        public void LoadScene(string sceneName)
        {
            PreviousSceneName = CurrentSceneName;
            CurrentSceneName = sceneName;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        public void LoadAddScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        public void LoadSceneAsync(string sceneName, Action onEndCallback)
        {
            PreviousSceneName = CurrentSceneName;
            CurrentSceneName = sceneName;
            var handle = KosherUnityCoroutineManager.StartCoroutine(ProcessLoadScene(sceneName), () =>
            {
                onEndCallback?.Invoke();
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
