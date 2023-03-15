using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Godot;

namespace Scanner.ImagePipeline
{
    public class ImageMagickToGodotImage_Properties : IModuleProperties
    {
        public int Width;
        public int Height;
        public int Channels;
    }

    public class ImageMagickToGodotImage : IModule<MagickImage, Image, ImageMagickToGodotImage_Properties>
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public TimeSpan LastRunTime { get; set; }
        public ModuleStatus Status { get; set; }
        public ImageMagickToGodotImage_Properties Properties { get; set; }

        public void Init()
        {
            Name = this.GetType().Name;
            Label = "ImageMagick image to Godot image";
            Description = "Converts the image from an ImageMagick image to a Godot format Image";
        }

        async Task<Image> IModule<MagickImage, Image, ImageMagickToGodotImage_Properties>.Run(MagickImage magickImage, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                var pixelMapping = PixelMapping.RGB;
                Image.Format format = Image.Format.Rgb8;
                if (Properties.Channels == 4)
                {
                    pixelMapping = PixelMapping.RGBA;
                    format = Image.Format.Rgba8;
                }
                byte[] byteArray = magickImage.GetPixels().ToByteArray(pixelMapping);
                return Image.CreateFromData(magickImage.Width, magickImage.Height, false, format, byteArray);
            }, token);
        }
    }
}
