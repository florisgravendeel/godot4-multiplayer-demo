using Godot;
using System;
using Multiplayerdemo.World;

public partial class MainMenu : Node2D
{
	private const int Port = 25541;
	private ENetMultiplayerPeer ENetMultiplayerPeer { get; set; }	
	private Button JoinButton { get; set; }
	private Button HostButton { get; set; }
	private LineEdit InputField { get; set; }
	private Node2D Level { get; set; }
	
	public override void _Ready()
	{
		Position = Vector2.Zero; // Position Main menu over the level.
		Level = GetParent().GetNode<Node2D>("Level"); 
		Level.Hide(); // Hide the level.
		
		JoinButton = GetNode<Button>("Redwall/JoinGameButton");
		JoinButton.Pressed += OnJoinButtonPressed; 
		HostButton = GetNode<Button>("Redwall/HostGameButton");
		HostButton.Pressed += OnHostButtonPressed;
		ENetMultiplayerPeer = new ENetMultiplayerPeer();
		InputField = GetNode<LineEdit>("Redwall/IPAddress");
	}

	private void OnJoinButtonPressed()
	{
		var address = InputField.Text;
		ENetMultiplayerPeer.CreateClient(address, Port);
		Multiplayer.MultiplayerPeer = ENetMultiplayerPeer;
		Hide();
		Level.Show();
	}

	private void OnHostButtonPressed()
	{
		ENetMultiplayerPeer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = ENetMultiplayerPeer;
		ENetMultiplayerPeer.PeerConnected += ENetMultiplayerPeerOnPeerConnected;
		Hide();
		Level.Show();
	}

	private void ENetMultiplayerPeerOnPeerConnected(long id)
	{
		var scene = (PackedScene)ResourceLoader.Load("res://Player/player.tscn");
		var player = (Player)scene.Instantiate();
		player.Name = id.ToString();
		player.Position = new Vector2(140, 287);
		GetNode<Node2D>("%Level").AddChild(player);
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
