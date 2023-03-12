using System;
using System.Threading;


public interface ICancellable
{
    public CancellationTokenSource cancellationTokenSource {get;}

    public void Cancel(){
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }
}
