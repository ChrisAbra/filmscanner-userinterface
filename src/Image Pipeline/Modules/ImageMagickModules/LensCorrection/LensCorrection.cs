
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline.ImageMagickModules
{
    public class LensCorrection : IImageMagickModule, IBypassablePipelineModule<MagickImage>
    {
        public string Name { get => "ImageMagickLensCorrection"; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Bypass { get; set; }

        public LensCorrection()
        {
            this.Label = "ImageMagick Lens correction";
            this.Description = "Uses the ImageMagick Library to perform Lens Correction";
        }

        public async Task<MagickImage> Run(MagickImage image)
        {
            await Task.Run(() =>
            {
                //image.Distort(DistortMethod.Barrel);
            });
            return image;
        }
    }
}