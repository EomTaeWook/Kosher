using System;
using System.Collections;

namespace KosherUnity.Coroutine
{
    public class CoroutineHandle : IEnumerator
    {
        public object Current => enumerator.Current;

        private IEnumerator enumerator;
        private Action onCompleteCallback;

        public CoroutineHandle(IEnumerator enumerator, Action onCompleteCallback) 
        {
            this.enumerator = enumerator;
            this.onCompleteCallback = onCompleteCallback;
        }        

        public bool Stop()
        {
            return IsRunning && KosherUnityCoroutineManager.Instance.Stop(this);
        }

        public bool IsRunning
        {
            get { return enumerator != null && KosherUnityCoroutineManager.Instance.IsRunning(this); }
        }
        
        public IEnumerator Wait()
        {
            if (enumerator != null)
            {
                while (KosherUnityCoroutineManager.Instance.IsRunning(this))
                {
                    yield return null;
                }
            }
        }

        public bool MoveNext()
        {
            var isFinished = !enumerator.MoveNext();
            if(isFinished == true)
            {
                onCompleteCallback?.Invoke();
            }
            return !isFinished;
        }

        public void Reset()
        {
            enumerator.Reset();
        }
    }
}
