using Godot;
using System;

namespace Scanner.UI {
	public partial class ImageViewport : Panel
	{
		GlobalSignals GlobalSignals;

		TextureRect ImageRenderer;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
			ImageRenderer = GetNode<TextureRect>("%ImageRenderer");
			GlobalSignals.ImagePipelineCompletedImage += (image) => RenderImage(image);
		}

		public void RenderImage(Image image)
		{
			image.Convert(Image.Format.Rgb8);
			ImageRenderer.Texture = ImageTexture.CreateFromImage(image);
		}
	}
}
