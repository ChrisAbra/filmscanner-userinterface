using Godot;
using System;

namespace Scanner.ImagePipeline
{
    public partial class Exposure : ImageModule
    {
        public partial class ExposureProperties : ModuleProperties
        {
            [HSliderProperty("Exposure Amount (EV)", 1f, 0f, 5f)]
            public float EV { get; set; }

            [HSliderProperty("Test other slider", 2f, 0f, 5f)]
            public float TestSlider { get; set; }
        }

        public Exposure(string label, ExposureProperties properties)
        {
            this.Name = "Exposure";
            this.Label = label ?? this.Name;
            this.Description = "Controlls the Exposure of an image";
            this.ModuleProperties = properties;
        }

        protected override Image Process(Image image)
        {
            if (ModuleProperties is ExposureProperties properties)
            {
                image.AdjustBcs(properties.EV, 1f, 1f);
                return image;
            }
            else
            {
                throw new Exception("Properties provided are not of compatible type");
            }
        }
    }
}
