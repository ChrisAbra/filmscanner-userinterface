using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline
{
    public static class MagickImageExtension
    {
        public static async Task<PipelineImage> ToPipelineImage(this MagickImage magickImage, ColorSpace colorSpace = ColorSpace.RGB)
        {
            var img = new PipelineImage();
            await Task.Run((Action)(() =>
            {
                if (magickImage.Format != MagickFormat.Exr)
                {
                    magickImage.Format = MagickFormat.Exr;
                }

                magickImage.SetBitDepth(16);
                img.PixelByteArray = magickImage.GetPixels().ToByteArray(0, 0, magickImage.Width, magickImage.Height, PixelMapping.RGB);
                img.Width = magickImage.Width;
                img.Height = magickImage.Height;
                img.ColorSpace = magickImage.TranslateColorSpaceEnumToPipeline(colorSpace);
            }));
            return img;
        }

        public static async Task<MagickImage> FromPipelineImage(this MagickImage magickImage, PipelineImage pipelineImage)
        {
            await Task.Run(() =>
            {
                var settings = new PixelReadSettings(pipelineImage.Width, pipelineImage.Height, StorageType.Char, PixelMapping.RGB);
                magickImage.ReadPixels(pipelineImage.PixelByteArray, settings);
            });
            return magickImage;
        }

        public class ColorSpaceTranslateException : Exception
        {
            public ColorSpaceTranslateException(string message, Exception inner)
                : base(message, inner) { }
        }

        public static ColorSpace TranslateColorSpaceEnumToPipeline(this MagickImage magickImage, ColorSpace colorSpace)
        {
            string enumName = Enum.GetName(typeof(ColorSpace), colorSpace);
            try
            {
                return (ColorSpace)Enum.Parse(typeof(ColorSpace), enumName);
            }
            catch (Exception e)
            {
                throw new ColorSpaceTranslateException("Could not translate from the ImageMagick color space definitions to Pipeline colour space definition", e);
            }
        }

        public static ImageMagick.ColorSpace TranslateColorSpaceEnumToImageMagick(this MagickImage magickImage, ColorSpace colorSpace)
        {
            string enumName = Enum.GetName(typeof(ColorSpace), colorSpace);
            try
            {
                return (ImageMagick.ColorSpace)Enum.Parse(typeof(ImageMagick.ColorSpace), enumName);
            }
            catch (Exception e)
            {
                throw new ColorSpaceTranslateException("Could not translate from the ImageMagick color space definitions to Pipeline colour space definition", e);
            }
        }
    }
}