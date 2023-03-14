
using System;
using ImageMagick;

namespace Scanner.ImagePipeline.ImageMagickModules
{
    public class Orientation : IImageMagickModule
    {
        public struct Properties : IModuleProperties
        {
            public int ClockwiseRotations;
            public void RotateClockwise(){
                if(ClockwiseRotations > 3){
                    ClockwiseRotations = 0;
                } else{
                    ClockwiseRotations++;
                }
            }
            public void RotateAntiClockwise(){
                if(ClockwiseRotations == 0){
                    ClockwiseRotations = 3;
                } else{
                    ClockwiseRotations--;
                }
            }
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
            this.Description = "Uses the ImageMagick Library to perform orthagonal orientations";
            this.InputProperties = new Properties();
        }

        MagickImage IAsyncTimedPipelineModule<MagickImage>.Run(MagickImage input)
        {
            if(InputProperties is Properties props){
                if(props.ClockwiseRotations != 0){
                    int numberRotations = props.ClockwiseRotations % 4;
                    input.Rotate(numberRotations * 90);
                }
            }
            return input;
        }
    }
}