using Godot;
using System;


namespace Scanner.UI
{


    public partial class GlobalSignals : Node
    {
        [Signal]
        public delegate void OpenFileNotificationEventHandler(String filePath);

        public void EmitOpenFileNotification(String filePath){
            EmitSignal(SignalName.OpenFileNotification, filePath);
        }
    }

}
