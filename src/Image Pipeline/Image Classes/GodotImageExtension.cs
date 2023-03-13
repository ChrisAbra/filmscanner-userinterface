using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace Scanner.ImagePipeline
{
    public static class GodotImageExtension
    {
        public static async Task<Image> FromPipelineImage(this Image godotImage, PipelineImage pipelineImage, Image.Format imageFormat = Image.Format.Rgb8, ColorSpace colorSpace = ColorSpace.sRGB)
        {
            await Task.Run(async () =>
            {
                await ConvertColorSpace.Convert(pipelineImage, colorSpace);

                godotImage = Image.CreateFromData(pipelineImage.Width, pipelineImage.Height, false, imageFormat, pipelineImage.PixelByteArray);
            });
            return godotImage;
        }
    }
}