using System;
using System.Threading;

namespace Scanner.ImagePipeline
{
    public interface ICancellable
    {
        public CancellationTokenSource CancelTokenSource { get; }

        public void Cancel()
        {
            CancelTokenSource?.Cancel();
            CancelTokenSource?.Dispose();
        }
    }
}