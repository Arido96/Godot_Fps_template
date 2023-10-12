using Godot;
using System;

public partial class audio_controller : AudioStreamPlayer
{
	public void OnTimerTimeOut()
	{
		QueueFree();
	}
}
