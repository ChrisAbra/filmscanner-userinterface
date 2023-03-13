using System;
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline
{
    public static class ConvertColorSpace
    {
        public enum ConversionMethod
        {
            IMAGE_MAGICK
        }

        public static async Task<PipelineImage> Convert(PipelineImage image, ColorSpace targetColorSpace, ConversionMethod conversionMethod = ConversionMethod.IMAGE_MAGICK)
        {
            switch (conversionMethod)
            {
                case ConversionMethod.IMAGE_MAGICK:
                    {
                        await ConvertWithImageMagick(image, targetColorSpace);
                        break;
                    }
            }
            return image;
        }

        private static async Task<PipelineImage> ConvertWithImageMagick(PipelineImage image, ColorSpace targetColorSpace)
        {
            MagickImage magickImage = new();
            // convert to ImagickImage
            await magickImage.FromPipelineImage(image);
            //Translate the color space from ImagickType to Project type
            ImageMagick.ColorSpace targetMagickColorSpace = magickImage.TranslateColorSpaceEnumToImageMagick(targetColorSpace);
            //Convert the colour space with Imagick
            magickImage.ColorSpace = targetMagickColorSpace;
            //Reconvert back to PipelineImage
            return await magickImage.ToPipelineImage(targetColorSpace);
        }
    }
}