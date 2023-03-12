using System;
using System.Threading.Tasks;


public interface IBypassablePipelineModule<InputType, OutputType> : IPipelineModule<InputType, OutputType>
    where InputType : class
    where OutputType : class

{
    public Boolean Bypass { get; set; }

    public Task<OutputType> BypassedOutput(InputType input);
}
