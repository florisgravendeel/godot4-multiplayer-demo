using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	// Scale up the window if we are not playing on mobile
	public override void _Ready()
	{
		if (OS.GetName() != "iOS" && OS.GetName() != "Android")
		{
			GetTree().Root.Size = new Vector2i(1920, 1080);
		}
	}

}
