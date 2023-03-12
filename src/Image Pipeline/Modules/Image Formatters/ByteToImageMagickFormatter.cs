using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

public class ByteToImageMagickFormatter : IPipelineModule<byte[], MagickImage>
{
    public string name { get => "ImageMagickFileFormatter"; }

    public string label { get; set; }
    public string description { get; set; }

    public ByteToImageMagickFormatter(){
        Init();
    }

    public void Init()
    {
        this.label = "ImageMagick Converter";
        this.description = "Takes the read file and converts it to an ImageMagick element to enable ImageMagick based operations";
    }

    public async Task<MagickImage> Run(byte[] byteArray)
    {
        var img = new MagickImage();
        await Task.Run(() =>
        {
            img.Read(byteArray);
        });
        return img;
    }
}