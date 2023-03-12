using Godot;
using System;


namespace Scanner.UI
{
    public partial class OpenableFile : Node
    {

        [Export]
        String filePath;


        public void OpenFile()
        {
            GD.Print(filePath);
			GetNode<GlobalSignals>("/root/GlobalSignals")?.EmitOpenFileNotification(filePath);
        }

    }

}
