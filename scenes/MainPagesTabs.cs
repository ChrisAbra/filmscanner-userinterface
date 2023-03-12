using Godot;
using System;

namespace Scanner.UI
{

    public partial class MainPagesTabs : TabContainer
    {
        // Called when the node enters the scene tree for the first time.

		[Export]
		Control developElement;
        public override void _Ready()
        {
            GetNode<GlobalSignals>("/root/GlobalSignals").OpenFileNotification += (filePath) =>
            {
                selectDevelopTabWhenImageLoaded();
            };
        }

        public void selectDevelopTabWhenImageLoaded(){
			this.CurrentTab = this.GetTabIdxFromControl(developElement);
		}
    }

}
