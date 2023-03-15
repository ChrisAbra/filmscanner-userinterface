using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline {
    public class FileToImageMagick_Properties : IModuleProperties {}

    public class FileToImageMagick : IModule<string, MagickImage, FileToImageMagick_Properties>
    {
        public string Name {get; set;}

        public string Label {get;set;}

        public string Description {get;set;}

        public TimeSpan LastRunTime {get;set;}
        public ModuleStatus Status {get;set;}
        public FileToImageMagick_Properties Properties {get;set;}

        public void Init()
        {
            Name = this.GetType().Name;
            Label = "File to Image Magick";
            Description = "Reads a provided file path into an ImageMagick Image";
        }

        public FileToImageMagick(){
            Init();
        }

        async Task<MagickImage> IModule<string, MagickImage, FileToImageMagick_Properties>.Run(string filePath, CancellationToken token)
        {
            var image = new MagickImage();
            await image.ReadAsync(filePath, token);
            return image;
        }
    }
}
