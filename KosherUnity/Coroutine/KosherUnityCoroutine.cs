using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KosherUnity
{
    public class KosherUnityCoroutine
    {
        private Action<object> onCompleteCallback;
        private bool isFinished;
        private object obj;
        public static KosherUnityCoroutine StartCoroutine(IEnumerator enumerator, Action<object> onCompleteCallback)
        {
            var coroutineData = new KosherUnityCoroutine
            {
                onCompleteCallback = onCompleteCallback,
                obj = enumerator,
            };

            coroutineData.Process(enumerator);

            return coroutineData;
        }
        public static KosherUnityCoroutine StartCoroutine(AsyncOperation asyncOperation, Action<object> onCompleteCallback)
        {
            var coroutineData = new KosherUnityCoroutine
            {
                onCompleteCallback = onCompleteCallback,
                obj = asyncOperation
            };
            coroutineData.Process(asyncOperation);
            return coroutineData;
        }
        public bool Process()
        {


            return true;
        }
        private IEnumerator Process(AsyncOperation asyncOperation)
        {
            while (isFinished == false)
            {
                isFinished = asyncOperation.isDone;
                yield return null;
            }
            onCompleteCallback?.Invoke(obj);
        }
        private IEnumerator Process(IEnumerator enumerator)
        {
            while (isFinished == false)
            {
                isFinished = enumerator.MoveNext();

                yield return enumerator.Current;
            }
            onCompleteCallback?.Invoke(obj);
        }
    }
}
