using Godot;
using Scanner.ImagePipeline;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.UI
{
    public partial class Develop : Control
    {
        GlobalSignals GlobalSignals;
        Tree treeNode;

        private Dictionary<string, PixelPipeline> FilePipelines;

        public PixelPipeline ActivePipeline;

        private CancellationTokenSource PipelineCancellationTokenSource;

        public override void _Ready()
        {
            GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
            treeNode = GetNode<Tree>("%PixelPipeline");
            FilePipelines = new();
            AttachToSignals();
        }

        private void AttachToSignals()
        {
            GlobalSignals.OpenFileNotification += (filePath) => LoadFile(filePath);
            GlobalSignals.ImagePipelineUpdatedProperty += (moduleName, properties) => SetPipelineProperty(moduleName, properties);
        }

        public async void LoadFile(string filePath)
        {
            ActivePipeline = FilePipelines.GetValueOrDefault(filePath) ?? new PixelPipeline(filePath);
            FilePipelines.TryAdd(filePath, ActivePipeline);
            await RunActivePipeline();
        }

        public async void SetPipelineProperty(string moduleName, ModuleProperties properties)
        {
            GD.Print(moduleName);
            GD.Print(properties);
            ActivePipeline.UpdateModuleProperties(moduleName, properties);
            await RunActivePipeline();
        }

        public async Task RunActivePipeline()
        {
            PipelineCancellationTokenSource?.Cancel();

            PipelineCancellationTokenSource = new CancellationTokenSource();
            Image image = await ActivePipeline.RunPipeline(PipelineCancellationTokenSource.Token);
            if(image != null){
                GlobalSignals.EmitSignal(GlobalSignals.SignalName.ImagePipelineCompletedImage, image);
                PipelineCancellationTokenSource.Cancel();
            }
        }
    }
}
