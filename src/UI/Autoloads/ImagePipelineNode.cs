using Godot;
using System;
using Scanner.ImagePipeline;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using ImageMagick;
using System.Collections.Generic;

namespace Scanner.UI
{
    public partial class ImagePipelineNode : Node
    {
        public enum Status
        {
            NO_FILE,
            LOADING,
            LOADING_FINISHED,
            WORKING,
            COMPLETE
        }
        public Status status = Status.NO_FILE;

        public CancellationTokenSource FileAccessCancellationSource { get; private set; }

        public TreeItem[] pipelineTreeItems;

        private byte[] activeFileByteArray;
        private string activeFilePath;
        private CancellationToken activeToken;

        public Image godotImage;

        private List<IImageMagickModule> ImageMagickPipelineModules;
        private List<IComputeShaderModule> ComputeShaderModules;

        GlobalSignals GlobalSignals;

        public override void _Ready()
        {
            GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
            SetupDefaultModules();

            GlobalSignals.ImagePipelineStatusUpdate += (status) => HandleStatusUpdates(status);
        }

        public async void HandleStatusUpdates(int status){
            switch((Status)status) {
                case Status.LOADING_FINISHED: {
                    await RunPipeline(activeToken);
                    break;
                }
            }
        }

        public void SetupDefaultModules()
        {
            ImageMagickPipelineModules = new List<IImageMagickModule>();
            ComputeShaderModules = new List<IComputeShaderModule>();

            ImagePipeline.ImageMagickModules.Orientation orientation = new()
            {
                InputProperties = new ImagePipeline.ImageMagickModules.Orientation.Properties
                {
                    ClockwiseRotations = 1
                }
            };
            ImageMagickPipelineModules.Add(orientation);
        }

        public async Task LoadFile(String filePath, CancellationToken token)
        {
            activeToken = token;
            if(filePath == this.activeFilePath){
                return;
            }
            this.UnloadFile();
            this.UpdateStatus(Status.LOADING);
            this.activeFileByteArray = await File.ReadAllBytesAsync(filePath, token);
            this.activeFilePath = filePath;
            this.UpdateStatus(Status.LOADING_FINISHED);
        }

        private void UnloadFile()
        {
            this.activeFileByteArray = null;
            this.godotImage = null;
            this.activeFilePath = null;
            this.UpdateStatus(Status.NO_FILE);
        }

        public class PipelineError
        {
            public enum Type
            {
                CANCELLED
            }
            public string message;
            public Type type;

            public PipelineError(Type type, string message)
            {
                this.type = type;
                this.message = message;
            }
        }

        public async Task<PipelineError> RunPipeline(CancellationToken token)
        {
            if (token.IsCancellationRequested) return PipelineCancellationError();

            this.UpdateStatus(Status.WORKING);
            //Convert from byte[] to ImageMagick

            MagickImageInfo imageInfo = new ();
            imageInfo.Read(activeFilePath);
            MagickImage magickImage = new ();
            await magickImage.ReadAsync(new MemoryStream(activeFileByteArray),imageInfo.Format, token);
            GD.Print("Magick Image. Image width", magickImage.Width);

            //Run through ImageMagick items
            foreach (var module in ImageMagickPipelineModules)
            {
                if (token.IsCancellationRequested) return PipelineCancellationError();
                magickImage = await module.Run(magickImage);
            }

            //Convert to PipelineImage
            if (token.IsCancellationRequested) return PipelineCancellationError();
            var pipelineImage = await magickImage.ToPipelineImage();
            GD.Print("Pipeline Image. Image width", pipelineImage.Width);

            //Run through Compute Shader items
            foreach (var module in ComputeShaderModules)
            {
                if (token.IsCancellationRequested) return PipelineCancellationError();
                pipelineImage = await module.Run(pipelineImage);
            }

            //Convert to Godot Image
            if (token.IsCancellationRequested) return PipelineCancellationError();
            this.godotImage = await new Image().FromPipelineImage(pipelineImage);

            GD.Print("Godot Image. Image width", godotImage.GetWidth());

            this.UpdateStatus(Status.COMPLETE);

            GlobalSignals.EmitImagePipelineImageResult(godotImage);

            return null;
        }

        private void UpdateStatus(Status status)
        {
            this.status = status;
            GlobalSignals.EmitImagePipelineUpdateStatus(status);
        }

        private static PipelineError PipelineCancellationError()
        {
            return new PipelineError(PipelineError.Type.CANCELLED, "Pipeline cancelled");
        }
    }
}
