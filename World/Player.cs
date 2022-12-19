using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Multiplayerdemo.World;

public partial class Player : CharacterBody2D
{
	public const float Speed = 330.0f;
	public const float JumpVelocity = -500.0f;
	public AnimatedSprite2D[] AnimatedSprite2D { get; set; }
	public Skins Skin { get; set; }

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public const float Gravity = 980;

	public override void _Ready()
	{
		SetSkin(Skins.MaskFrog);
		AnimatedSprite2D = GetChildren().OfType<AnimatedSprite2D>().ToArray();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			AnimatedSprite.Animation = "fall";	
			velocity.y += Gravity * (float)delta;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			AnimatedSprite.Animation = "jump";	
			velocity.y = JumpVelocity;
		}

		Vector2 direction = Input.GetVector("left", "right", "jump", "down");
		if (direction != Vector2.Zero)
		{
			AnimatedSprite.Animation = "run";
			velocity.x = direction.x * Speed;
			AnimatedSprite.FlipH = direction.x < 0;
			
		}
		else
		{
			AnimatedSprite.Animation = "idle";
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public AnimatedSprite2D AnimatedSprite => AnimatedSprite2D[(int)Skin];

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("change_skin1"))
		{
			SetSkin(Skins.MaskFrog);
		} else if (@event.IsActionPressed("change_skin2"))
		{
			SetSkin(Skins.VirtualGuy);
		} else if (@event.IsActionPressed("change_skin3"))
		{
			SetSkin(Skins.PinkMan);
		}
	}

	private void SetSkin(Skins skin)
	{
		GetNode<Label>("PlayerTag").Text = skin.ToString();
		var list = GetChildren().OfType<AnimatedSprite2D>().ToList();
		list.ForEach(x =>
		{
			if (x.Name.Equals(skin.ToString())) 
				x.Show();
			else
				x.Hide();
		});
	}
	
	public enum Skins
	{
		MaskFrog = 0,
		VirtualGuy = 1,
		PinkMan = 2,
	}
}
