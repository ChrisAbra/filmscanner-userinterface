using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline {
    public interface IPipelineModule<InputType,OutputType>
    {
        public string Name {get;}
        public string Label {get;set;}
        public string Description {get;set;}

        public Task<OutputType> Run(InputType input);
    }
}
