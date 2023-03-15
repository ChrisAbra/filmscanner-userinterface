using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline
{
    public class FloatArrayToImageMagick_Properties : IModuleProperties
    {
        public int Width;
        public int Height;
        public int Channels;
    }

    public class FloatArrayToImageMagick : IModule<float[], MagickImage, FloatArrayToImageMagick_Properties>
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public TimeSpan LastRunTime { get; set; }
        public ModuleStatus Status { get; set; }
        public FloatArrayToImageMagick_Properties Properties { get; set; }

        public void Init()
        {
            Name = this.GetType().Name;
            Label = "ImageMagick image to Float array";
            Description = "Reads the Pixels to a Float Array";
        }

        async Task<MagickImage> IModule<float[], MagickImage, FloatArrayToImageMagick_Properties>.Run(float[] floatArray, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                var pixelMapping = PixelMapping.RGB;
                if (Properties.Channels == 4)
                {
                    pixelMapping = PixelMapping.RGBA;
                }
                var settings = new PixelReadSettings(Properties.Width, Properties.Height, StorageType.Quantum, pixelMapping);
                var image = new MagickImage();
                image.ReadPixels(floatArray, settings);
                return image;
            }, token);
        }
    }
}
