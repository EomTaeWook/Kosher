using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using KosherUtils.Coroutine.Interface;

namespace KosherUtils.Coroutine
{
    public class CoroutineWoker : ICoroutineWoker
    {
        private List<IEnumerator> workers = new List<IEnumerator>();
        private List<float> delays = new List<float>();

        public CoroutineHandle Start(float delay, IEnumerator enumerator, Action onCompleteCallback = null)
        {
            var handle = new CoroutineHandle(this, enumerator, onCompleteCallback);
            workers.Add(handle);
            delays.Add(delay);

            return handle;
        }
        public CoroutineHandle Start(IEnumerator enumerator, Action onCompleteCallback = null)
        {
            return Start(0, enumerator, onCompleteCallback);
        }

        public bool IsRunning(IEnumerator enumerator)
        {
            for (int i = 0; i < workers.Count; ++i)
            {
                if (workers[i] == enumerator)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Stop(IEnumerator enumerator)
        {
            for (int i = 0; i < workers.Count; ++i)
            {
                if (workers[i] == enumerator)
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
        public bool WorkerUpdate(float deltaTime)
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
                    else if (MoveNext(workers[i], i) == false)
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
        private bool MoveNext(IEnumerator enumerator, int index)
        {
            if (enumerator.Current is IEnumerator)
            {
                if (MoveNext((IEnumerator)enumerator.Current, index) == true)
                {
                    return true;
                }
                delays[index] = 0f;
            }

            bool result = enumerator.MoveNext();
            if (enumerator.Current is float)
            {
                delays[index] = (float)enumerator.Current;
            }
            return result;
        }
    }
}
