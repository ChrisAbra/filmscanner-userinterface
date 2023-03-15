using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using ImageMagick;

namespace Scanner.ImagePipeline
{
    public enum PixelPipelineProcessingStatus // Status of the processing for the whole pipeline
    {
        INITIALISING,
        LOADING,
        LOADING_FINISHED,
        WORKING,
        COMPLETE
    }
    public enum PixelPipelineState // State this pipeline is in
    {
        ACTIVE,
        DORMANT,
        THUMBNAIL_ONLY,
        UNLOADED
    }

    public class PixelPipeline
    {
        public PixelPipelineProcessingStatus Status = PixelPipelineProcessingStatus.INITIALISING;

        public PixelPipelineState State
        {
            get { return state; }
            set
            {
                switch (value)
                {
                    case PixelPipelineState.ACTIVE:
                        {
                            break;
                        }
                    case PixelPipelineState.DORMANT:
                        {
                            CompletedImage = null;
                            break;
                        }
                    case PixelPipelineState.THUMBNAIL_ONLY:
                        {
                            CompletedImage = null;
                            UnprocessedImage = null;
                            break;
                        }
                    case PixelPipelineState.UNLOADED:
                        {
                            CompletedImage = null;
                            UnprocessedImage = null;
                            Thumbnail = null;
                            break;
                        }
                }
                state = value;
            }
        }
        private PixelPipelineState state;
        public Image CompletedImage;

        public Image UnprocessedImage;

        public Node SignalNode;

        public Image Thumbnail;

        private readonly string filePath;

        private List<ImageModule> ImageProcessingModules;

        public List<TreeItem> TreeItems;

        public PixelPipeline(string filePath, PixelPipelineState state = PixelPipelineState.ACTIVE)
        {
            this.filePath = filePath;
            this.state = state;
            SetupDefaultPipelineModules();
        }

        public void SetupDefaultPipelineModules()
        {
            ImageProcessingModules = new()
            {
                new Exposure("Exposure")
            };
        }

        public async Task<Image> RunPipeline()
        {
            return await RunPipeline(0, false, 0);
        }

        public async Task<Image> RunPipeline(int pipelineStartIndex, bool cache, int pipelineCacheIndex)
        {
            State = PixelPipelineState.ACTIVE;
            if (UnprocessedImage == null)
            {
                DateTime loadTimeStart = DateTime.Now;
                await LoadFile();
                GD.Print("LoadTime : " + (DateTime.Now - loadTimeStart));
            }

            //if no modules have been modified since last run then return cached image;
            bool hasDirtyModules = false;
            foreach (var module in ImageProcessingModules)
            {
                if (module.Status == ModuleStatus.DIRTY)
                {
                    hasDirtyModules = true;
                    break;
                }
            }

            if (!hasDirtyModules && CompletedImage != null)
            {
                GD.Print("Return cached image");
                return CompletedImage;
            }

            Image image = new();
            image.CopyFrom(UnprocessedImage);
            //Runs the pipeline from the start index 
            for (int i = pipelineStartIndex; i < ImageProcessingModules.Count; i++)
            {
                var module = ImageProcessingModules[i];
                if (module.IsDisabled)
                {
                    continue;
                }
                if (i == pipelineStartIndex && module.CachedImage != null)
                {
                    image = module.CachedImage;
                    continue;
                }
                bool cacheModule = cache && i == pipelineCacheIndex;
                image = module.CachedImage ?? await Task.Run(() => module.Run(ref image, cacheModule));
                GD.Print(module.Label + " : " + module.LastRunTime);
            }

            DateTime to24bitStartTime = DateTime.Now;
            await Task.Run(() => RGBFImageToRGB8(ref image));
            CompletedImage = image;
            GD.Print("ConvertTo24bit image : " + (DateTime.Now - to24bitStartTime));

            return CompletedImage;
        }

        private void RGBFImageToRGB8(ref Image image)
        {
            var settings = new PixelReadSettings(image.GetWidth(), image.GetHeight(), StorageType.Quantum, PixelMapping.RGB);
            var magickImage = new MagickImage(image.GetData(), settings);

            byte[] byteArray = magickImage.GetPixels().ToByteArray(PixelMapping.RGB);
            image.SetData(magickImage.Width, magickImage.Height, false, Image.Format.Rgb8, byteArray);
            magickImage.Dispose();
        }

        private async Task<Image> LoadFile()
        {
            if (filePath == null)
            {
                return null;
            }
            Status = PixelPipelineProcessingStatus.LOADING;

            var magickImage = new MagickImage();
            await magickImage.ReadAsync(filePath);

            var magickImagePixels = magickImage.GetPixels();

            if (magickImagePixels.Channels == 4)
            {
                magickImage.Alpha(AlphaOption.Remove);
            }
            float[] floatArray = magickImagePixels.ToArray();
            byte[] byteArray = new byte[floatArray.Length * 4];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);

            UnprocessedImage = Image.CreateFromData(magickImage.Width, magickImage.Height, false, Image.Format.Rgbf, byteArray);
            magickImage.Dispose();
            Status = PixelPipelineProcessingStatus.LOADING_FINISHED;
            return UnprocessedImage;
        }

        public void ClearCachedImages()
        {
            this.CompletedImage = null;
            this.UnprocessedImage = null;
        }
    }
}