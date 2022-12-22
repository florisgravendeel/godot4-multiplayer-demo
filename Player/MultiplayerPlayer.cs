using Godot;
using System;
using Multiplayerdemo.World;

public partial class MultiplayerPlayer : Node2D
{
	private MultiplayerSynchronizer MultiplayerSynchronizer { get; set; }

	public Vector2 SyncPosition;

	public override void _EnterTree()
	{
		
		MultiplayerSynchronizer = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
		MultiplayerSynchronizer.SetMultiplayerAuthority(int.Parse(GetParent<Player>().Name));
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
