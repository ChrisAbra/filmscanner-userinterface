using Godot;
using Scanner.ImagePipeline;

namespace Scanner.UI
{

    public partial class ExposureUI : Node
    {
        GlobalSignals GlobalSignals;
        HSlider EVSlider;
        public Exposure.ExposureProperties Properties;
        public override void _Ready()
        {
            GlobalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
            EVSlider = GetNode<HSlider>("%EVSlider");
            Properties = new Exposure.ExposureProperties(){
                EV = 1f
            };
            Properties.GetPropertyList();
            EVSlider.Value = Properties.EV;
        }

        public void OnEVSliderValueChange(double EV){
            Properties.EV = (float)EV;
            GlobalSignals.EmitSignal(GlobalSignals.SignalName.ImagePipelineUpdatedProperty,"Exposure",Properties);
        }
    }

}
