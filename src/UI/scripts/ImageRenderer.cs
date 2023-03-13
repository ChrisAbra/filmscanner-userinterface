using Godot;
using System;

namespace Scanner.UI
{
    public partial class ImageRenderer : TextureRect
    {
		private int currentZoomAmount = 1;
		public void Zoom(int amount){
			this.Scale = new Vector2(amount,amount);
		}

		public void ResetZoom(){
			this.Zoom(1);
		}
    }
}
