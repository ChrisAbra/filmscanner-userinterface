using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public static class GodotImageExtension {

    public static async Task<Image> FromPipelineImage(this Image godotImage, PipelineImage pipelineImage, Image.Format imageFormat = Image.Format.Rgb8, PipelineImage.ColorSpace colorSpace = PipelineImage.ColorSpace.sRGB){
        await Task.Run(async () => {

            await ConvertColorSpace.Convert(pipelineImage,colorSpace);

            godotImage = Image.CreateFromData(pipelineImage.width, pipelineImage.height, false, imageFormat, pipelineImage.pixelByteArray);
            
        });
        return godotImage;
    }
}
