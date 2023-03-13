using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline
{
    public interface IComputeShaderModule : IBypassablePipelineModule<PipelineImage>
    {
        public string Name { get; }

        public string Label { get; }
        public string Description { get; }

        public TimeSpan LastRunTime { get; set; }

        public IModuleProperties InputProperties { get; set; }

        public async Task<PipelineImage> RunAsync(PipelineImage input, CancellationToken cancellationToken)
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

        protected PipelineImage Run(PipelineImage input);
    }
}
