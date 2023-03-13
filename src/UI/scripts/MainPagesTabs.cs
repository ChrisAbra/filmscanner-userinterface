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
            GetNode<GlobalSignals>("/root/GlobalSignals").OpenFileNotification += (_) => SelectDevelopTabWhenImageLoaded();
        }

        public void SelectDevelopTabWhenImageLoaded(){
			this.CurrentTab = this.GetTabIdxFromControl(developElement);
		}
    }
}
