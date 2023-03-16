using System;
using System.Reflection;
using Godot;
using Scanner.ImagePipeline;

namespace Scanner.UI
{
    public abstract partial class ModuleUserInterface : Control
    {
        public ModuleProperties Properties;

        public void CreateNodeForProperties()
        {
            foreach(var property in Properties.GetType().GetProperties()){
                var attribute = property.GetCustomAttribute<ImageModulePropertyAttribute>();
                if(attribute != null){
                    AddChild(attribute.CreateNode());
                }
            }
        }
    }
}
