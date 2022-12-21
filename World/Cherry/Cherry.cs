using Godot;
using System;
using Multiplayerdemo.World;

public partial class Cherry : Node2D
{
	private Area2D Area2D { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area2D = GetNode<Area2D>("Area2D");
		Area2D.BodyEntered += OnBodyEntered;
		
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Floating");
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			Area2D.BodyEntered -= OnBodyEntered; // Disable event as we want this method only to execute once.
			CollectCherry();
		}
	}

	private void CollectCherry()
	{
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play("default");
		animatedSprite.AnimationFinished += () => QueueFree();
		
	}
}
