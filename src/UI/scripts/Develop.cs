using Godot;
using System;

namespace Scanner.UI
{

    public partial class Develop : Control
    {

        private FileToByteArray fileReader;
        private ByteToImageMagickFormatter byteToImageMagickFormatter;
        private ImageMagickToPipelineImageFormatter imageMagickToPipelineFormatter;
        private GodotImageFormatter godotImageFormatter;

        private TextureRect textureRect;

        public override void _Ready()
        {
            fileReader = new FileToByteArray();
            byteToImageMagickFormatter = new ByteToImageMagickFormatter();
            imageMagickToPipelineFormatter = new ImageMagickToPipelineImageFormatter();
            godotImageFormatter = new GodotImageFormatter();
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

            var fileByteArray = await fileReader.ReadFileAsync(filePath);

            var img_magickImage = await byteToImageMagickFormatter.Run(fileByteArray);

            var img = await imageMagickToPipelineFormatter.Run(img_magickImage);

            var img_godot = await godotImageFormatter.Run(img);

            textureRect.Texture = ImageTexture.CreateFromImage(img_godot);

        }


        private void setupPipeline()
        {

        }
    }
}
