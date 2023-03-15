using Godot;
using ImageMagick;
using System;
using System.Threading;
using System.Threading.Tasks;
using Scanner.ImagePipeline;
using System.Collections.Generic;

namespace Scanner.UI
{
    public partial class Develop : Control
    {
        GlobalSignals GlobalSignals;

        private Dictionary<String, PixelPipeline> FilePipelines;

        //private CancellationTokenSource PipelineCancellationTokenSource;

        public override void _Ready()
        {
            GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
            FilePipelines = new ();
            AttachToSignals();
        }

        private void AttachToSignals()
        {
            GlobalSignals.OpenFileNotification += (filePath) => LoadFile(filePath);
        }

        public async void LoadFile(string filePath)
        {
            PixelPipeline activePipeline = FilePipelines.GetValueOrDefault(filePath) ?? new PixelPipeline(filePath);
            FilePipelines.TryAdd(filePath, activePipeline);
            Image image = await activePipeline.RunPipeline();

            GlobalSignals.EmitSignal(GlobalSignals.SignalName.ImagePipelineCompletedImage, image);
        }
    }
}
