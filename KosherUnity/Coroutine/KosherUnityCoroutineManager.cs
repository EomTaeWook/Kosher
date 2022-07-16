using KosherUtils.Coroutine;
using System;
using System.Collections;
using UnityEngine;

namespace KosherUnity.Coroutine
{
    public class KosherUnityCoroutineManager : SingletonWithMonoBehaviour<KosherUnityCoroutineManager>
    {
        private static CoroutineWoker coroutineWoker = new CoroutineWoker();
        public static CoroutineHandle StartCoroutine(IEnumerator enumerator, Action onCompleteCallback)
        {
            return StartCoroutine(0, enumerator, onCompleteCallback);
        }
        public static CoroutineHandle StartCoroutine(float delay, IEnumerator enumerator, Action onCompleteCallback)
        {
            var handle = new CoroutineHandle(coroutineWoker, enumerator, onCompleteCallback);
            return handle;
        }
        void Update()
        {
            coroutineWoker.WorkerUpdate(Time.deltaTime);
        }
    }
}
