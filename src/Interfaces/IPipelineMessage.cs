using Godot;
using System;


public interface IPipelineMessage
{
}

public class ImagePipelineMessage : IPipelineMessage
{
    public Image Img { get; set;}
}
public class FilePathPipelineMessage : IPipelineMessage
{
    public String FilePath { get; set;}

}
public class ByteArrayPipelineMessage : IPipelineMessage
{
    public byte[] ByteArray { get; set;}
    public int imageWidth;
    public int imageHeight;

}
