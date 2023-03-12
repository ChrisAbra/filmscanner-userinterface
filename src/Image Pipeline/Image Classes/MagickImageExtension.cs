using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

public static class MagickImageExtension {

    public static async Task<PipelineImage> ToPipelineImage(this MagickImage magickImage, ColorSpace colorSpace = ColorSpace.RGB )
    {
        var img = new PipelineImage();
        await Task.Run(() =>
        {
            if(magickImage.Format != MagickFormat.Exr){
                magickImage.Format = MagickFormat.Exr;
            }

            magickImage.SetBitDepth(16);
            img.pixelByteArray = magickImage.GetPixels().ToByteArray(0,0,magickImage.Width,magickImage.Height,PixelMapping.RGB);
            img.width = magickImage.Width;
            img.height = magickImage.Height;
            img.colorSpace = magickImage.TranslateColorSpaceEnumToPipeline(colorSpace);
        });
        return img;
    }

    public static async Task<MagickImage> FromPipelineImage(this MagickImage magickImage, PipelineImage pipelineImage){

        await Task.Run(() => {
            var settings = new PixelReadSettings(pipelineImage.width,pipelineImage.height,StorageType.Char,PixelMapping.RGB);
            magickImage.ReadPixels(pipelineImage.pixelByteArray,settings);
        });
        return magickImage;
    }

    public class ColorSpaceTranslateException : Exception
    {
        public ColorSpaceTranslateException(string message, Exception inner)
            : base(message, inner) {}
    }

    public static PipelineImage.ColorSpace TranslateColorSpaceEnumToPipeline(this MagickImage magickImage, ColorSpace colorSpace){
        String enumName = Enum.GetName(typeof(ImageMagick.ColorSpace), colorSpace);
        try{
            PipelineImage.ColorSpace pipelineColorSpace = (PipelineImage.ColorSpace)Enum.Parse(typeof (PipelineImage.ColorSpace), enumName);
            return pipelineColorSpace;
        }
        catch(Exception e){
            throw new ColorSpaceTranslateException("Could not translate from the ImageMagick color space definitions to Pipeline colour space definition",e);
        }
    }

    public static ColorSpace TranslateColorSpaceEnumToImageMagick(this MagickImage magickImage, PipelineImage.ColorSpace colorSpace){
        String enumName = Enum.GetName(typeof(PipelineImage.ColorSpace), colorSpace);
        try{
            ColorSpace magickColorSpace = (ColorSpace)Enum.Parse(typeof (ColorSpace), enumName);
            return magickColorSpace;
        }
        catch(Exception e){
            throw new ColorSpaceTranslateException("Could not translate from the ImageMagick color space definitions to Pipeline colour space definition",e);
        }
    }


}
