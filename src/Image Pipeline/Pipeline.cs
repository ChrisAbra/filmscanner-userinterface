using Godot;
using ImageMagick;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline
{
    public class Pipeline
    {
        public PipelineStatus Status;

        public int ImageWidth;
        public int ImageHeight;
        public int ImageChannels;
        public string ImageFilePath;
        public float[] Pixels;

        public bool FileLoaded;

        public Image CompletedImage;

        public IModule<string, MagickImage, FileToImageMagick_Properties> FileReaderToImageModule;
        public List<IModule<MagickImage, MagickImage, IModuleProperties>> MagickImageModules;

        public IModule<MagickImage, float[], ImageMagickToFloatArray_Properties> ImageToFloatModule;
        public List<IModule<float[], float[], IModuleProperties>> PixelToPixelModules;
        public IModule<float[], MagickImage, FloatArrayToImageMagick_Properties> PixelsToImageModule;
        public IModule<MagickImage, Image, ImageMagickToGodotImage_Properties> ImageToGodotImage;

        public Pipeline()
        {
            // Sets up the default inbound and display bound transformations
            FileReaderToImageModule = new FileToImageMagick();
            ImageToFloatModule = new ImageMagickToFloatArray();
            PixelsToImageModule = new FloatArrayToImageMagick();
            ImageToGodotImage = new ImageMagickToGodotImage();

            // Initiialises the list of pixel operations
            PixelToPixelModules = new List<IModule<float[], float[], IModuleProperties>>();
            MagickImageModules = new List<IModule<MagickImage, MagickImage, IModuleProperties>>();
        }

        public void RegisterModules(IModuleProperties properties){
            
        }
        public async Task<Image> GetDisplayableImageFromPipeline(string filePath, CancellationToken cancellationToken)
        {
            GD.Print("===================================");
            GD.Print(filePath);
            GD.Print(this.ImageFilePath);
            GD.Print(this.Status);
            if (filePath == this.ImageFilePath && this.Status == PipelineStatus.COMPLETE)
            {
                GD.Print("Already rendered image");
                return CompletedImage;
            }
            await ReadFile(filePath, cancellationToken);
            UpdateStatus(PipelineStatus.WORKING);

            foreach (var module in PixelToPixelModules)
            {
                if (module.Status != ModuleStatus.DISABLED)
                {
                    Pixels = await module.RunTimed(Pixels, cancellationToken);
                }
            }

            MagickImage magickImage = await PixelsToImageModule.RunTimed(Pixels, cancellationToken);

            foreach (var module in MagickImageModules)
            {
                if (module.Status != ModuleStatus.DISABLED)
                {
                    magickImage = await module.RunTimed(magickImage, cancellationToken);
                }
            }

            CompletedImage = await ImageToGodotImage.RunTimed(magickImage, cancellationToken);
            UpdateStatus(PipelineStatus.COMPLETE);
            return CompletedImage;
        }

        public async Task ReadFile(string filePath, CancellationToken cancellationToken)
        {
            this.FileLoaded = false;
            this.ImageFilePath = filePath;
            UpdateStatus(PipelineStatus.LOADING);
            MagickImage image = await FileReaderToImageModule.RunTimed(this.ImageFilePath, cancellationToken);

            this.ImageWidth = image.Width;
            this.ImageHeight = image.Height;
            this.ImageChannels = image.GetPixels().Channels;

            this.Pixels = await ImageToFloatModule.RunTimed(image, cancellationToken);
            this.FileLoaded = true;

            this.PixelsToImageModule.Properties = new FloatArrayToImageMagick_Properties
            {
                Width = this.ImageWidth,
                Height = this.ImageHeight,
                Channels = this.ImageChannels
            };

            this.ImageToGodotImage.Properties = new ImageMagickToGodotImage_Properties
            {
                Width = this.ImageWidth,
                Height = this.ImageHeight,
                Channels = this.ImageChannels
            };

            UpdateStatus(PipelineStatus.LOADING_FINISHED);

        }

        public void UnloadImage(){
            CompletedImage = null;
            FileLoaded = false;
        }

        private void UpdateStatus(PipelineStatus newStatus)
        {
            this.Status = newStatus;
            GD.Print(newStatus);
        }
    }
}