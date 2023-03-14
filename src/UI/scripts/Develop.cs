using Godot;
using ImageMagick;
using System;
using System.Threading;
using System.Threading.Tasks;
using Scanner.ImagePipeline;

namespace Scanner.UI
{
    public partial class Develop : Control
    {
        GlobalSignals GlobalSignals;

        private Pipeline activePipeline;

        private CancellationTokenSource PipelineCancellationTokenSource;

        public override void _Ready()
        {
            GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
			AttachToSignals();
        }

		private void AttachToSignals(){
			GlobalSignals.OpenFileNotification += (filePath) => LoadFile(filePath);
		}

        public async void LoadFile(string filePath)
        {
            PipelineCancellationTokenSource?.Cancel();
            PipelineCancellationTokenSource = new ();
            activePipeline = new Pipeline();
            Image image = await activePipeline.GetDisplayableImageFromPipeline(filePath, PipelineCancellationTokenSource.Token);
            GlobalSignals.EmitSignal(GlobalSignals.SignalName.ImagePipelineCompletedImage, image);

        }
    }
}
