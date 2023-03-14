using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline
{
    public interface IAsyncTimedPipelineModule<T> : IBypassablePipelineModule<T>
        where T : class
    {
        public string Name { get; }

        public string Label { get; }
        public string Description { get; }

        public TimeSpan LastRunTime { get; set; }

        public async Task<T> RunAsync(T input, CancellationToken cancellationToken)
        {
            DateTime startTime = DateTime.Now;
            var output = input;
            if (!Bypass)
            {
                output = await Task.Run(() => Run(input), cancellationToken);
            }
            LastRunTime = DateTime.Now - startTime;
            return output;
        }

        protected T Run(T input);
    }
}
