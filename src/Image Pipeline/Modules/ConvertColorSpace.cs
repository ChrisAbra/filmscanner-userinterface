using System;
using System.Threading.Tasks;
using ImageMagick;

public class ConvertColorSpace
{

    public enum ConversionMethod
    {
        IMAGE_MAGICK
    }

    public static async Task<PipelineImage> Convert(PipelineImage image, PipelineImage.ColorSpace targetColorSpace, ConversionMethod conversionMethod = ConversionMethod.IMAGE_MAGICK)
    {

        switch(conversionMethod){
            case ConversionMethod.IMAGE_MAGICK: {
                image =  await ConvertWithImageMagick(image, targetColorSpace);
                break;
            }       
        }
        return image;

    }

    private static async Task<PipelineImage> ConvertWithImageMagick(PipelineImage image, PipelineImage.ColorSpace targetColorSpace)
    {
        MagickImage magickImage = new MagickImage();
        await magickImage.FromPipelineImage(image);
        ColorSpace targetMagickColorSpace = magickImage.TranslateColorSpaceEnumToImageMagick(targetColorSpace);
        magickImage.ColorSpace = targetMagickColorSpace;
        image = await magickImage.ToPipelineImage(targetMagickColorSpace);
        return image;

    }
}