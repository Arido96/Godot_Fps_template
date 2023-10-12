using Godot;
using System;

public partial class AdsController : Node3D
{
    [Export] public Vector3 AimDownSightOffset { get; private set; } //Adjust gun Aim Down Sight position
    [Export] GunController _gunController;

    private Vector3 _initalPosition;

    public bool isAdsing { get; private set; }  

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _initalPosition = _gunController.RecoilReciever.Position;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionPressed("ads"))
        {
            AimDownSight();
            isAdsing= true;
        }
        else
        {
            Hip();
            isAdsing = false;
        }

    }

    public void AimDownSight()
    {
        var tmpPos = _gunController.RecoilReciever.Position.Lerp(AimDownSightOffset, 0.5f);
        _gunController.RecoilReciever.Position = tmpPos;
    }
    public void Hip()
    {
        var tmpPos = _gunController.RecoilReciever.Position.Lerp(_initalPosition, 0.5f);
        _gunController.RecoilReciever.Position = tmpPos;
    }




}
