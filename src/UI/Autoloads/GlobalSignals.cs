using Godot;
using System;

namespace Scanner.UI
{
    public partial class GlobalSignals : Node
    {
        // File IO signals
        [Signal]
        public delegate void OpenFileNotificationEventHandler(String filePath);

        public void EmitOpenFileNotification(String filePath){
            EmitSignal(SignalName.OpenFileNotification, filePath);
        }

        //ImagePipeline Signals
        [Signal]
        public delegate void ImagePipelineStatusUpdateEventHandler(int status);
        [Signal]
        public delegate void ImagePipelineCompletedImageEventHandler(Image image);

        public void EmitImagePipelineUpdateStatus(ImagePipelineNode.Status status){
            EmitSignal(SignalName.ImagePipelineStatusUpdate, (int)status);
        }
        public void EmitImagePipelineImageResult(Image image){
            EmitSignal(SignalName.ImagePipelineCompletedImage, image);
        }
    }
}
