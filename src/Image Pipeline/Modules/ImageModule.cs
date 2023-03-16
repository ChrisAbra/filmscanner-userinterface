using Godot;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline
{
    public abstract partial class ModuleProperties : GodotObject { }
    public enum ModuleStatus
    {
        DIRTY,
        DISABLED,
        WORKING,
        COMPLETE
    }
    public abstract partial class ImageModule
    {
        public ModuleStatus Status;

        public bool IsDisabled;

        public string Name;
        public string Label;
        public string Description;

        public TimeSpan LastRunTime;

        public Image CachedImage;

        protected abstract Image Process(Image image);

        public ModuleProperties ModuleProperties;

        public Image Run(ref Image input, bool cacheImage)
        {
            this.Status = ModuleStatus.WORKING;
            DateTime startTime = DateTime.Now;
            Image output = Process(input);
            LastRunTime = DateTime.Now - startTime;
            this.Status = ModuleStatus.COMPLETE;
            if (cacheImage)
            {
                CachedImage = new Image();
                CachedImage.CopyFrom(output);
            }
            else
            {
                CachedImage = null;
            }
            return output;
        }

        public Control CreateUIControls(){
            Control container = new (){
                Name = Label
            };
            foreach(var property in ModuleProperties.GetType().GetProperties()){
                var attribute = property.GetCustomAttribute<ImageModulePropertyAttribute>();
                container.AddChild(attribute.CreateNode());
            }
            return container;
        }
    }
}
