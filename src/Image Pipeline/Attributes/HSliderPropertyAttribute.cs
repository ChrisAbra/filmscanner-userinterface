using System;
using Godot;

namespace Scanner.ImagePipeline
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HSliderPropertyAttribute : ImageModulePropertyAttribute
    {
        public float DefaultValue {get;}
        public float Min {get;}
        public float Max {get;}
        public HSliderPropertyAttribute(string label, float defaultValue, float min, float max) : base(label) {
            this.DefaultValue = defaultValue;
            this.Min = min;
            this.Max = max;
        }

        public override HSlider CreateNode(){
            var slider = new HSlider
            {
                MinValue = Min,
                MaxValue = Max,
                Value = DefaultValue
            };
            //slider.Connect(HSlider.SignalName.ValueChanged,onChange);
            return slider;
        }
    }
}
