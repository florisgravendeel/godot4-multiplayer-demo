using Godot;

namespace Multiplayerdemo.Player;

public partial class MultiplayerPlayer : Node
{
	private MultiplayerSynchronizer MultiplayerSynchronizer { get; set; }
	
	public override void _Ready()
	{
		MultiplayerSynchronizer = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
	}

	private Vector2 _syncPosition;
	[Export] public Vector2 SyncPosition
	{
		get => _syncPosition;
		set
		{
			if (MultiplayerSynchronizer.IsMultiplayerAuthority())
			{
				_syncPosition = value;
			}
			else
			{
				GetParent<World.Player>().Position = value;
			}
		}
	}
	
	// private string _syncAnimation;
	// [Export] public string SyncAnimation
	// {
	// 	get => _syncAnimation;
	// 	set
	// 	{
	// 		if (MultiplayerSynchronizer.IsMultiplayerAuthority())
	// 		{
	// 			_syncAnimation = value;
	// 		}
	// 		else
	// 		{
	// 			GetParent<World.Player>().CurrentSkin.Animation = value;
	// 		}
	// 	}
	// }
	
	private bool _syncFlipH;
	[Export] public bool SyncFlipH
	{
		get => _syncFlipH;
		set
		{
			if (MultiplayerSynchronizer.IsMultiplayerAuthority())
			{
				_syncFlipH = value;
			}
			else
			{
				GetParent<World.Player>().CurrentSkin.FlipH = value;
			}
		}
	}

}