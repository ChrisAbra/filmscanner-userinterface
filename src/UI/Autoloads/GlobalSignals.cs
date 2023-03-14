using Godot;
using System;

namespace Scanner.UI
{
    public partial class GlobalSignals : Node
    {
        // File IO signals
        [Signal]
        public delegate void OpenFileNotificationEventHandler(String filePath);

        //ImagePipeline Signals
        [Signal]
        public delegate void ImagePipelineStatusUpdateEventHandler(int status);
        [Signal]
        public delegate void ImagePipelineCompletedImageEventHandler(Image image);
    }
}
