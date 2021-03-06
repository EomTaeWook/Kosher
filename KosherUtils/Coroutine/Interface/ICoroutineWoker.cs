using System;
using System.Collections;
namespace KosherUtils.Coroutine.Interface
{
    public interface ICoroutineWoker
    {
        bool IsRunning(IEnumerator enumerator);
        bool Stop(IEnumerator enumerator);
        void StopAll();
        CoroutineHandle Start(IEnumerator enumerator, Action onCompleteCallback);
    }
}
