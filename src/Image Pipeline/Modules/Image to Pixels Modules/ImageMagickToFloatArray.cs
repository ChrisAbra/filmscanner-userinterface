using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline {

    public struct ImageMagickToFloatArray_Properties : IModuleProperties {}

    public class ImageMagickToFloatArray : IModule<MagickImage, float[], ImageMagickToFloatArray_Properties>
    {
        public string Name {get; set;}

        public string Label {get;set;}

        public string Description {get;set;}

        public TimeSpan LastRunTime {get;set;}
        public ModuleStatus Status {get;set;}
        public ImageMagickToFloatArray_Properties Properties {get;set;}

        public void Init()
        {
            Name = this.GetType().Name;
            Label = "ImageMagick image to Float array";
            Description = "Reads the Pixels to a Float Array";
        }

        public ImageMagickToFloatArray(){ Init();}

        async Task<float[]> IModule<MagickImage, float[], ImageMagickToFloatArray_Properties>.Run(MagickImage image, CancellationToken token)
        {
            return await Task.Run(() => image.GetPixels().ToArray(), token);
        }
    }
}
