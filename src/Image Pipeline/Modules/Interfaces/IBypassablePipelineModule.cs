namespace Scanner.ImagePipeline
{
    public interface IBypassablePipelineModule<InputType>
        where InputType : class
    {
        public bool Bypass { get; set; }

        public InputType BypassedOutput(InputType input)
        {
            return input;
        }
    }
}