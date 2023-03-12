using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;


public class GodotImageFormatter : IPipelineModule<PipelineImage, Image>
{
    public string name { get => "ImageMagickFileFormatter"; }

    public string label { get; set; }
    public string description { get; set; }

    public GodotImageFormatter(){
        Init();
    }

    public void Init()
    {
        this.label = "ImageMagick Converter";
        this.description = "Takes the read file and converts it to an ImageMagick element to enable ImageMagick based operations";
    }

    public async Task<Image> Run(PipelineImage pipelineImage)
    {
        Image image = new Image();

        await Task.Run(() =>
        {
            if(pipelineImage?.format == PipelineImage.Format.BMP){
                image.LoadBmpFromBuffer(pipelineImage.byteArray);
            }
        });

        return image;
    }
}