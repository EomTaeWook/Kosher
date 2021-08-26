using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using KosherUtils.Coroutine.Interface;

namespace KosherUtils.Coroutine
{
    public class CoroutineHandle : IEnumerator
    {
        private ICoroutineWoker coroutineWoker;
        private IEnumerator enumerator;
        private Action onCompleteCallback;
        public CoroutineHandle(ICoroutineWoker coroutineWoker, IEnumerator enumerator, Action onCompleteCallback = null)
        {
            this.coroutineWoker = coroutineWoker;
            this.enumerator = enumerator;
            this.onCompleteCallback = onCompleteCallback;
        }
        public bool Stop()
        {
            return IsRunning && coroutineWoker.Stop(enumerator);
        }
        public IEnumerator Wait()
        {
            if (enumerator != null)
            {
                while (coroutineWoker.IsRunning(enumerator))
                {
                    yield return null;
                }
            }
        }
        public bool MoveNext()
        {
            var isFinished = !enumerator.MoveNext();
            if (isFinished == true)
            {
                onCompleteCallback?.Invoke();
            }
            return !isFinished;
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        public bool IsRunning
        {
            get { return enumerator != null && coroutineWoker.IsRunning(enumerator); }
        }

        public object Current => enumerator.Current;
    }
}
