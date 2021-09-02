using KosherUtils.Coroutine.Interface;
using System;
using System.Collections;

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
            return IsRunning && coroutineWoker.Stop(this);
        }
        public IEnumerator Wait()
        {
            if (enumerator != null)
            {
                while (coroutineWoker.IsRunning(this))
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
            get { return enumerator != null && coroutineWoker.IsRunning(this); }
        }

        public object Current => enumerator.Current;
    }
}
