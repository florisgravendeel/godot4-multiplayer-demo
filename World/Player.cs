using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Multiplayerdemo.World;

public partial class Player : CharacterBody2D
{
	public const float Speed = 330.0f;
	public const float JumpVelocity = -500.0f;
	public AnimatedSprite2D AnimatedSprite2D { get; set; }

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public const float Gravity = 980;

	public override void _Ready()
	{
		GD.Print(Gravity);
		AnimatedSprite2D = GetNode<AnimatedSprite2D>("MaskFrog");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			AnimatedSprite2D.Animation = "fall";	
			velocity.y += Gravity * (float)delta;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			AnimatedSprite2D.Animation = "jump";	
			velocity.y = JumpVelocity;
		}

		Vector2 direction = Input.GetVector("left", "right", "jump", "down");
		if (direction != Vector2.Zero)
		{
			AnimatedSprite2D.Animation = "run";
			velocity.x = direction.x * Speed;
			AnimatedSprite2D.FlipH = direction.x < 0;
			
		}
		else
		{
			AnimatedSprite2D.Animation = "idle";
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
