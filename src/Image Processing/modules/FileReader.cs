using Godot;
using System;
using ImageMagick;

public partial class FileReader : IPipelineModule
{
    public string Name { get => "FileReader"; }


    public bool Complete { get; set; }
    public bool Dirty { get; set; }
    public bool Active { get; set; }

    public IPipelineMessage Compute(IPipelineMessage input)
    {
        ByteArrayPipelineMessage output = new ByteArrayPipelineMessage();

        if (!(input is FilePathPipelineMessage))
        {
            return null;
        }

        FilePathPipelineMessage filePathMessage = input as FilePathPipelineMessage;
        String filePath = filePathMessage.FilePath;

        GD.Print(filePath);

        using (var image = new MagickImage(filePath))
        {
            var godotImg = new Image();
            image.Format = MagickFormat.Bmp;
            GD.Print(image.Depth);
            output.ByteArray = image.ToByteArray(image.Format);
            output.imageHeight = image.Height;
            output.imageWidth = image.Width;
        }


        return output;
    }

    public void Init()
    {
        this.Complete = false;
        this.Dirty = false;
        this.Active = true;
    }

    public void Invalidate()
    {
        throw new NotImplementedException();
    }
}
