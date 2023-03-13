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
        private ImagePipelineNode ImagePipelineNode;

        private CancellationTokenSource PipelineCancellationTokenSource;

        public override void _Ready()
        {
            GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
            ImagePipelineNode = GetNode<ImagePipelineNode>("/root/ImagePipelineNode");
			AttachToSignals();
        }

		private void AttachToSignals(){
			GlobalSignals.OpenFileNotification += (filePath) => LoadFile(filePath);
		}

        public async void LoadFile(string filePath)
        {
            PipelineCancellationTokenSource?.Cancel();
            PipelineCancellationTokenSource = new ();
            await ImagePipelineNode.LoadFile(filePath, PipelineCancellationTokenSource.Token);
        }
    }
}
