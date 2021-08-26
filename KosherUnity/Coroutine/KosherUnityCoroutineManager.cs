using KosherUtils.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KosherUnity.Coroutine
{
    public class KosherUnityCoroutineManager : SingletonWithMonoBehaviour<KosherUnityCoroutineManager>
    {
        private List<IEnumerator> workers = new List<IEnumerator>();
        private List<float> delays = new List<float>();
        public static CoroutineHandle StartCoroutine(IEnumerator enumerator, Action onCompleteCallback)
        {
            return StartCoroutine(0, enumerator, onCompleteCallback);
        }
        public static CoroutineHandle StartCoroutine(float delay, IEnumerator enumerator, Action onCompleteCallback)
        {
            var handle = new CoroutineHandle(enumerator, onCompleteCallback);

            KosherUnityCoroutineManager.Instance.workers.Add(handle);
            KosherUnityCoroutineManager.Instance.delays.Add(delay);
            return handle;
        }

        public bool Update(float deltaTime)
        {
            if (workers.Count > 0)
            {
                for (int i = 0; i < workers.Count; i++)
                {
                    if (delays[i] > 0f)
                    {
                        delays[i] -= deltaTime;
                    }
                    else if (workers[i] == null)
                    {
                        workers.RemoveAt(i);
                        delays.RemoveAt(i);
                        i--;
                    }
                    else if(MoveNext(workers[i], i) == false)
                    {
                        workers.RemoveAt(i);
                        delays.RemoveAt(i);
                        i--;
                    }
                }
                return true;
            }
            return false;
        }

        public bool IsRunning(IEnumerator routine)
        {
            for(int i=0; i< workers.Count; ++i)
            {
                if(workers[i] == routine)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Stop(IEnumerator routine)
        {
            for(int i=0; i<workers.Count; ++i)
            {
                if(workers[i] == routine)
                {
                    workers[i] = null;
                    return true;
                }
            }
            return false;
        }
        public void StopAll()
        {
            workers.Clear();
            delays.Clear();
        }
        void Update()
        {
            Update(Time.deltaTime);
        }
        private bool MoveNext(IEnumerator routine, int index)
        {
            if (routine.Current is IEnumerator)
            {
                if (MoveNext((IEnumerator)routine.Current, index) == true)
                {
                    return true;
                }
                delays[index] = 0f;
            }

            bool result = routine.MoveNext();
            if (routine.Current is float)
            {
                delays[index] = (float)routine.Current;
            }
            return result;
        }
    }
}
