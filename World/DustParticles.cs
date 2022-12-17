using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Multiplayerdemo.World;

public partial class DustParticles : Node2D
{
	public List<CPUParticles2D> Particles { get; set; }
	public Player Player { get; set; }
	
	public override void _Ready()
	{
		Player = GetParent<Player>();
		Particles = GetChildren().OfType<CPUParticles2D>().ToList();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Player.AnimatedSprite2D.Animation.Equals("run") && Player.IsOnFloor())
			Particles.ForEach(i => i.Emitting = true);
		else
			Particles.ForEach(i => i.Emitting = false);

	}
}
