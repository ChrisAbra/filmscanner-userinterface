using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

public class ColorSpaceTranslateException : Exception
{
    public ColorSpaceTranslateException(string message, Exception inner)
        : base(message, inner) {}
}

public class ImageMagickToPipelineImageFormatter : IPipelineModule<MagickImage, PipelineImage>
{
    public string name { get => "ImageMagickFileFormatter"; }

    public string label { get; set; }
    public string description { get; set; }

    public ImageMagickToPipelineImageFormatter(){
        Init();
    }

    public void Init()
    {
        this.label = "ImageMagick Converter";
        this.description = "Takes the read file and converts it to an ImageMagick element to enable ImageMagick based operations";
    }

    public async Task<PipelineImage> Run(MagickImage magickImage)
    {
        var img = new PipelineImage();
        await Task.Run(() =>
        {
            magickImage.Format = MagickFormat.Bmp;
            img.byteArray = magickImage.ToByteArray();
            img.format = PipelineImage.Format.BMP;
            img.colorSpace = convertColorSpaceName(magickImage.ColorSpace);
        });
        return img;
    }

    private PipelineImage.ColorSpace convertColorSpaceName(ImageMagick.ColorSpace magickColorSpace){
        String enumName = Enum.GetName(typeof(ImageMagick.ColorSpace), magickColorSpace);
        try{
            PipelineImage.ColorSpace pipelineColorSpace = (PipelineImage.ColorSpace)Enum.Parse(typeof (PipelineImage.ColorSpace), enumName);
            return pipelineColorSpace;
        }
        catch(Exception e){
            throw new ColorSpaceTranslateException("Could not translate from the ImageMagick color space definitions to Pipeline colour space definition",e);
        }
    }
}