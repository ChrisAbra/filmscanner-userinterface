using System;
using System.Threading;
using System.Threading.Tasks;

public interface IPipelineModule<InputType,OutputType>
        where InputType : class
        where OutputType : class
{

    public String name {get;}
    public String label {get;set;}
    public String description {get;set;}

    public void Init();

    public Task<OutputType> Run(InputType input);


}
