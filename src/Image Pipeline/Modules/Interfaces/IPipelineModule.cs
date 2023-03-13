using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline {
    public interface IPipelineModule<InputType,OutputType>
    {
        public string Name {get;}
        public string Label {get;}
        public string Description {get;}

        public Task<OutputType> RunAsync(InputType input, CancellationToken cancellationToken);
    }
}
