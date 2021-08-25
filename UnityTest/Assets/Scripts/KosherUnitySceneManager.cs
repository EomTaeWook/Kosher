using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KosherUtils.Framework;
using UnityEngine.SceneManagement;
using System.Collections;

namespace UnityTest
{
    public class KosherUnitySceneManager : KosherUnity.SingletonWithMonoBehaviour<KosherUnitySceneManager>
    {
        private KosherUnityCoroutine kosherUnityCoroutine;
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
            //StartCoroutine(Process(asyncOperation));


            kosherUnityCoroutine = UnityTest.KosherUnityCoroutine.StartCoroutine(Process(asyncOperation), (o) =>
            {
                Debug.Log("LoadSceneAsync callback");
                onCallback?.Invoke();
            });

            //kosherUnityCoroutine.Process(asyncOperation);
        }
        private IEnumerator Process(AsyncOperation asyncOperation)
        {
            var test = asyncOperation;
            Debug.Log("Process LoadSceneAsync callback");
            while (test.isDone == false)
            {
                Debug.Log("Process asyncOperation.isDone");
                if (test.isDone == true)
                {
                    Debug.Log("Process LoadSceneAsync inner done"); 
                }
                yield return null;
            }
            Debug.Log("Process LoadSceneAsync done");
            yield break;
        }
    }
}
