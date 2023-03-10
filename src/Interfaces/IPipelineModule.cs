using Godot;
using System;

public interface IPipelineModule
{

    public String Name {get;}

    public Boolean Complete {get;set;}
    public Boolean Dirty {get;set;}
    public Boolean Active {get;set;}

    public void Invalidate();

    public void Init();


    public IPipelineMessage Compute(IPipelineMessage input);
}
