using Godot;

namespace Multiplayerdemo.Player;

public partial class MultiplayerPlayer : Node
{
	// private Vector2 _position;
	// [Export]
	// public Vector2 Position
	// {
	// 	get => _position;
	// 	set
	// 	{
	// 		if (IsMultiplayerAuthority())
	// 		{
	// 			_position = value;
	// 		}
	// 		else
	// 		{
	// 			GetParent<CharacterBody2D>().Position = value;
	// 		}
	// 	}
	// }
	[Export] public Vector2 SyncPosition = Vector2.Zero;
}