using Godot;

namespace Multiplayerdemo.Player;

public partial class MultiplayerPlayer : MultiplayerSynchronizer
{
	private Vector2 _position;
	[Export]
	public Vector2 Position
	{
		get => _position;
		set
		{
			if (IsMultiplayerAuthority())
			{
				_position = value;
			}
			else
			{
				GetParent<CharacterBody2D>().Position = value;
			}
		}
	}
}