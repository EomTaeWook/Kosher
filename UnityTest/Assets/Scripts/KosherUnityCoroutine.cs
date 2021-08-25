using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityTest
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
            Debug.Log("StartCoroutine IEnumerator Process");
            var ienum = coroutineData.Process(enumerator);
            
            return coroutineData;
        }
        public static KosherUnityCoroutine StartCoroutine(AsyncOperation asyncOperation, Action<object> onCompleteCallback)
        {
            var coroutineData = new KosherUnityCoroutine
            {
                onCompleteCallback = onCompleteCallback,
                obj = asyncOperation
            };
            Debug.Log("StartCoroutine AsyncOperation Process");
            coroutineData.Process(asyncOperation);

            return coroutineData;
        }
        
        public IEnumerator Process(AsyncOperation asyncOperation)
        {
            Debug.Log("Process");
  
            while (isFinished == false)
            {
                isFinished = asyncOperation.isDone;
                yield return null;
            }
            onCompleteCallback?.Invoke(obj);
            yield break;
        }
        private IEnumerator Process(IEnumerator enumerator)
        {
            Debug.Log("IEnumerator Process");
            while (isFinished == false)
            {
                isFinished = enumerator.MoveNext();

                yield return enumerator.Current;
            }
            onCompleteCallback?.Invoke(obj);
            yield break;
        }
    }
}
