using Godot;
using System;
using System.Collections.Generic;

public partial class PixelPipeline : Tree
{
    const String FILE_PATH = "/Users/chris/Documents/Projects/Film Scanner/User Interface/test_files/";

    public override void _Ready()
    {
		//StartPipeline(FILE_PATH);
	}
	public void StartPipeline(String filePath){

		FileReader fileReader = new FileReader();
		fileReader.Init();

		FilePathPipelineMessage message = new FilePathPipelineMessage();
		message.FilePath = filePath;
		IPipelineMessage fileReaderOutput = fileReader.Compute(message);
		if(fileReaderOutput is ByteArrayPipelineMessage byteArrayMessage){
			RenderBmpByteArray(byteArrayMessage.ByteArray);
		}
	}


	public void RenderBmpByteArray(byte[] byteArray){ 
			Image img = new Image();
			img.LoadBmpFromBuffer(byteArray);
			img.GenerateMipmaps();
			TextureRect renderer = GetNode<TextureRect>("%ImageRenderer");
			var imgTexture = ImageTexture.CreateFromImage(img);
			float imageProportion = img.GetWidth() / img.GetHeight();
			renderer.Texture = imgTexture;

	}

	public void _on_openexr_pressed(){
		String filePath = FILE_PATH + "openexr.exr";
		StartPipeline(filePath);
	}

	public void _on_sony_positive_pressed(){
		String filePath = FILE_PATH + "sony_positive.arw";
		StartPipeline(filePath);
	}

	public void _on_kodak_lad_pressed(){
		String filePath = FILE_PATH + "kodak_lad.cin";
		StartPipeline(filePath);
	}

}
