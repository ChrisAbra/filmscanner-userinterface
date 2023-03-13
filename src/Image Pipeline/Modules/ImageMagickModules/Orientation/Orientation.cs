
using System;
using ImageMagick;

namespace Scanner.ImagePipeline.ImageMagickModules
{
    public class Orientation : IImageMagickModule
    {
        public struct Properties : IModuleProperties
        {
            public int ClockwiseRotations;
        }

        public string Name { get => "ImageMagickOrientation"; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Bypass { get; set; }
        public TimeSpan LastRunTime {get;set;}
        public IModuleProperties InputProperties {get;set;}

        public Orientation()
        {
            this.Label = "ImageMagick Orientation";
            this.Description = "Uses the ImageMagick Library to perform Orientations";
            this.InputProperties = new Properties();
        }

        MagickImage IImageMagickModule.Run(MagickImage input)
        {
            if(InputProperties is Properties props){
                if(props.ClockwiseRotations > 0){
                    input.Rotate(props.ClockwiseRotations * 90);
                }
            }
            return input;
        }
    }
}