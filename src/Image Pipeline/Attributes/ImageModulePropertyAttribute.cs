using System;
using Godot;

namespace Scanner.ImagePipeline
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ImageModulePropertyAttribute : Attribute
    {
        public string Label {get;set;}

        protected ImageModulePropertyAttribute(string label){
            this.Label = label;
        }

        public abstract Control CreateNode();
    }
}
