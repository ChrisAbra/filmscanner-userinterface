
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline.ImageMagickModules
{
    public class Orientation : IImageMagickModule, IBypassablePipelineModule<MagickImage>
    {
        public struct Properties : IModuleProperties
        {
            public int ClockwiseRotations;
        }

        public string Name { get => "ImageMagickOrientation"; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Bypass { get; set; }

        public IModuleProperties InputProperties;

        public Orientation()
        {
            this.Label = "ImageMagick Orientation";
            this.Description = "Uses the ImageMagick Library to perform Auto Orientation";
            this.InputProperties = new Properties();
        }

        public async Task<MagickImage> Run(MagickImage image)
        {
            if (Bypass)
            {
                return image;
            }

            if (InputProperties is Properties properties)
            {
                if(properties.ClockwiseRotations > 0){
                    await Task.Run(() => image.Rotate(properties.ClockwiseRotations * 90));
                }
            }
            return image;
        }
    }
}