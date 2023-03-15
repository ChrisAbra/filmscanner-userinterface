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

        private Dictionary<String,Pipeline> FilePipelines;

        private CancellationTokenSource PipelineCancellationTokenSource;

        public override void _Ready()
        {
            GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
            FilePipelines = new ();
			AttachToSignals();
        }

		private void AttachToSignals(){
			GlobalSignals.OpenFileNotification += (filePath) => LoadFile(filePath);
		}

        public async void LoadFile(string filePath)
        {
            PipelineCancellationTokenSource?.Cancel();
            PipelineCancellationTokenSource = new ();
            Pipeline activePipeline = FilePipelines.GetValueOrDefault(filePath) ?? new Pipeline();
            Image image = await activePipeline?.GetDisplayableImageFromPipeline(filePath, PipelineCancellationTokenSource.Token);
            FilePipelines.TryAdd(filePath,activePipeline);
            GlobalSignals.EmitSignal(GlobalSignals.SignalName.ImagePipelineCompletedImage, image);
        }
    }
}
