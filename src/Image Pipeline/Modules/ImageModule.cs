using Godot;
using System;
using System.Threading.Tasks;

namespace Scanner.ImagePipeline
{
    public enum ModuleStatus {
        DIRTY,

        DISABLED,
        WORKING,
        COMPLETE
    }
    public abstract class ImageModule
    {
        public ModuleStatus Status;

        public bool IsDisabled;

        public string Name;
        public string Label;
        public string Description;

        public TimeSpan LastRunTime;

        public Image CachedImage;

        protected abstract Image Process(Image image);

        public Image Run(ref Image input, bool cacheImage)
        {
            this.Status = ModuleStatus.WORKING;
            DateTime startTime = DateTime.Now;
            Image output = Process(input);
            LastRunTime = DateTime.Now - startTime;
            this.Status = ModuleStatus.COMPLETE;
            if(cacheImage){
                CachedImage = new Image();
                CachedImage.CopyFrom(output);
            }
            else{
                CachedImage = null;
            }
            return output;
        }
    }
}
