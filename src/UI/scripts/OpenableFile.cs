using Godot;

namespace Scanner.UI
{
    public partial class OpenableFile : Node
    {
        [Export]
        string filePath;
        GlobalSignals GlobalSignals {get => GetNode<GlobalSignals>("/root/GlobalSignals");}

        public void OpenFile()
        {
            GD.Print(filePath);
            GlobalSignals?.EmitOpenFileNotification(filePath);
        }
    }
}
