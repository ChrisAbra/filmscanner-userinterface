
using System;
using ImageMagick;

namespace Scanner.ImagePipeline.ImageMagickModules
{
    public class Paint : IImageMagickModule
    {
        public struct Properties : IModuleProperties
        {}

        public string Name { get => "ImageMagickPaint"; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Bypass { get; set; }
        public TimeSpan LastRunTime {get;set;}
        public IModuleProperties InputProperties {get;set;}

        public Paint()
        {
            this.Label = "ImageMagick Paint";
            this.Description = "Uses the ImageMagick Library to perform Paint";
            this.InputProperties = new Properties();
        }

        MagickImage IImageMagickModule.Run(MagickImage input)
        {
            if(InputProperties is Properties props){}
            input.OilPaint();
            return input;
        }
    }
}