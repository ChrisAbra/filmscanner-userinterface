using Godot;
using ImageMagick;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline {
    public class Pipeline {
        public PipelineStatus Status;

        public int ImageWidth;
        public int ImageHeight;
        public int ImageChannels;
        public string ImageFilePath;
        public float[] Pixels;

        public bool FileLoaded;

        private CancellationTokenSource tokenSource;

        public IModule<string,MagickImage, FileToImageMagick_Properties> FileReaderToImageModule;
        public IModule<MagickImage,float[], ImageMagickToFloatArray_Properties> ImageToFloatModule;
        public List<IModule<float[],float[], IModuleProperties>> PixelToPixelModules;
        public IModule<float[],MagickImage, FloatArrayToImageMagick_Properties> PixelsToImageModule;
        public IModule<MagickImage,Image, ImageMagickToGodotImage_Properties> ImageToGodotImage;

        public Pipeline(){
            FileReaderToImageModule =  (IModule<string,MagickImage, FileToImageMagick_Properties>)new FileToImageMagick();
            ImageToFloatModule =  new ImageMagickToFloatArray();
            PixelsToImageModule =  new FloatArrayToImageMagick();
            ImageToGodotImage =  new ImageMagickToGodotImage();

            PixelToPixelModules = new List<IModule<float[],float[], IModuleProperties>>();
        }
        public async Task<Image> GetDisplayableImageFromPipeline(string filePath, CancellationToken cancellationToken){
            await ReadFile(filePath);
            return await GetDisplayableImageFromPipeline(cancellationToken);
        }

        public async Task<Image> GetDisplayableImageFromPipeline(CancellationToken cancellationToken){
            if(!FileLoaded && ImageFilePath != null){
                await ReadFile(cancellationToken);
            }

            updateStatus(PipelineStatus.WORKING);
            foreach(var module in PixelToPixelModules){
                if(module.Status != ModuleStatus.DISABLED){
                    await module.RunTimed(Pixels,cancellationToken);
                }
            }
            MagickImage magickImage = await PixelsToImageModule.RunTimed(Pixels,cancellationToken);
            Image image = await ImageToGodotImage.RunTimed(magickImage,cancellationToken);
            updateStatus(PipelineStatus.COMPLETE);
            return image;
        }
        public async Task<Image> GetDisplayableImageFromPipeline(){
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
            return await GetDisplayableImageFromPipeline(tokenSource.Token);
        }

        public async Task ReadFile(string filePath){
            FileLoaded = false;
            ImageFilePath = filePath;
            GD.Print(filePath);

            await ReadFile();
        }

        private async Task ReadFile(){
            tokenSource = new CancellationTokenSource();
            await ReadFile(tokenSource.Token);
            tokenSource.Dispose();
        }

        private async Task ReadFile(CancellationToken token){
            updateStatus(PipelineStatus.LOADING);
            MagickImage image = await FileReaderToImageModule.RunTimed(ImageFilePath,token);

            ImageWidth = image.Width;
            ImageHeight = image.Height;
            ImageChannels = image.GetPixels().Channels;
            GD.Print(ImageChannels);

            Pixels = await ImageToFloatModule.RunTimed(image,token);
            FileLoaded = true;

            PixelsToImageModule.Properties = new FloatArrayToImageMagick_Properties{
                Width = ImageWidth,
                Height = ImageHeight,
                Channels = ImageChannels
            };
            
            ImageToGodotImage.Properties = new ImageMagickToGodotImage_Properties{
                Width = ImageWidth,
                Height = ImageHeight,
                Channels = ImageChannels
            };

            updateStatus(PipelineStatus.LOADING_FINISHED);
        }

        private void updateStatus(PipelineStatus newStatus){
            GD.Print(newStatus);
            Status = newStatus;
        }
    }
}