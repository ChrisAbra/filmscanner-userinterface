using Godot;
using ImageMagick;
using System;
using System.Threading.Tasks;

namespace Scanner.UI
{

    public partial class Develop : Control
    {

        private FileToByteArray fileReader;
        private TextureRect textureRect;

        public override void _Ready()
        {
            fileReader = new FileToByteArray();
            textureRect = GetNode<TextureRect>("%ImageRenderer");

			attachToSignals();

        }

		private void attachToSignals(){
			GetNode<GlobalSignals>("/root/GlobalSignals").OpenFileNotification += (filePath) => {
				LoadFile(filePath);
			};
		}

        public async void LoadFile(String filePath)
        {

            var img_magickImage = new MagickImage();
            await img_magickImage.ReadAsync(filePath);

            var img = await img_magickImage.ToPipelineImage();

            GD.Print(img.width);
            GD.Print(img.colorSpace);
            
            var img_godot = new Image();
            img_godot = await img_godot.FromPipelineImage(img,Image.Format.Rgb8);

            GD.Print(img_godot.GetWidth());

            // sRGB color transformation happens here
            textureRect.Texture = ImageTexture.CreateFromImage(img_godot);

        }


        private void setupPipeline()
        {

        }
    }
}
