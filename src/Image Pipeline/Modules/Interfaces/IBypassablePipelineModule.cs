using System;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline
{
    public interface IBypassablePipelineModule<InputType>
        where InputType : class
    {
        public Boolean Bypass { get; set; }

        public InputType BypassedOutput(InputType input)
        {
            return input;
        }
    }
}