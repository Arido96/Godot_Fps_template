using Godot;
using System;

public partial class Main : Node3D
{




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _UnhandledInput(InputEvent inputEvent)
    {
       if(inputEvent.IsActionPressed("quit"))
		{
			GetTree().Quit();
		}
    }
}
