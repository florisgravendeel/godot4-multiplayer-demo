using System;
using System.Linq;
using Godot;

namespace Multiplayerdemo.World;

public partial class Player : CharacterBody2D
{
	private const float Speed = 330.0f;
	private const float JumpVelocity = -500.0f;
	public const float Gravity = 980;
	
	private AnimatedSprite2D[] AnimatedSprite2D { get; set; }
	public AnimatedSprite2D CurrentSkin => AnimatedSprite2D[(int)Skin];
	private Skins Skin { get; set; }
	
	private MultiplayerSynchronizer MultiplayerSynchronizer { get; set; }
	private MultiplayerPlayer MultiplayerPlayer { get; set; }
	
	public override void _Ready()
	{
		SetSkin(Skins.MaskFrog);
		AnimatedSprite2D = GetChildren().OfType<AnimatedSprite2D>().ToArray();
		MultiplayerPlayer = GetNode<MultiplayerPlayer>("Network");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!Multiplayer.GetUniqueId().ToString().Equals(Name)) // If the player is the local authority on the server
		{
			Position = MultiplayerPlayer.SyncPosition;
			return;
		}
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			CurrentSkin.Animation = "fall";	
			velocity.y += Gravity * (float)delta;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			CurrentSkin.Animation = "jump";	
			velocity.y = JumpVelocity;
		}

		Vector2 direction = Input.GetVector("left", "right", "jump", "down");
		if (direction != Vector2.Zero)
		{
			CurrentSkin.Animation = "run";
			velocity.x = direction.x * Speed;
			CurrentSkin.FlipH = direction.x < 0;
			
		}
		else
		{
			CurrentSkin.Animation = "idle";
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
		}

		Velocity = velocity;
		MultiplayerPlayer.SyncPosition = Position;
		MoveAndSlide();
	}


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
		Skin = skin;
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
