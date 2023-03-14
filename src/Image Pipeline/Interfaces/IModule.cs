using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline {

    public interface IModule <InputType,OutputType, PropertiesType>
        where PropertiesType : IModuleProperties
    {
        public string Name {get; protected set;}
        public string Label {get;}
        public string Description {get;}

        public TimeSpan LastRunTime { get; set; }

        public ModuleStatus Status {get; set;}

        public PropertiesType Properties { get; set; }

        public void Init();

        public async Task<OutputType> RunTimed(InputType input, CancellationToken cancellationToken)
        {
            this.Status = ModuleStatus.WORKING;
            DateTime startTime = DateTime.Now;
            var output = await Run(input, cancellationToken);
            LastRunTime = DateTime.Now - startTime;
            this.Status = ModuleStatus.COMPLETE;
            return output;
        }

        protected Task<OutputType> Run(InputType input, CancellationToken cancellationToken);
    }
}
