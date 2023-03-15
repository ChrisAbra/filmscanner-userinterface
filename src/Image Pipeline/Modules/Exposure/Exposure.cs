using Godot;
using System;

namespace Scanner.ImagePipeline
{
    public class Exposure : ImageModule
    {
        public Exposure(string label){
            this.Name = "Exposure";
            this.Label = label ?? this.Name;
            this.Description = "Controlls the Exposure of an image";
        }

        protected override Image Process(Image image)
        {
            return image;
        }
    }
}
