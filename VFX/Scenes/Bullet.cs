using Godot;
using System;

public partial class Bullet : Node3D
{

	[Export]private float _speed;


	private Vector3 dir;	
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position +=  Transform.Basis *new Vector3(_speed,0,0) * (float)delta;
	}

   private void  OnBodyEntered(Node3D body)
	{
		QueueFree();
	}
}
