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
		if (OS.GetName() != "iOS" && OS.GetName() != "Android")
		{
			GetTree().Root.Size = new Vector2i(2560, 1440);
		}
		
		Position = Vector2.Zero; // Position Main menu over the level.
		Level = GetNode<Node2D>("%Level"); 
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
		HideMenu();
	}

	private void OnHostButtonPressed()
	{
		ENetMultiplayerPeer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = ENetMultiplayerPeer;
		ENetMultiplayerPeer.PeerConnected += OnPeerConnected;
		ENetMultiplayerPeer.PeerDisconnected += OnPeerDisconnected;
		HideMenu();
		OnPeerConnected(1); // Let the host join the game. 
	}

	private void OnPeerDisconnected(long id)
	{
		Level.GetNode<Player>(id.ToString()).QueueFree();
	}

	private void HideMenu()
	{
		Hide();
		Position = new Vector2(3000, 3000);
		Level.Show();
	}

	private void OnPeerConnected(long id)
	{
		var scene = (PackedScene)ResourceLoader.Load("res://Player/player.tscn");
		var player = (Player)scene.Instantiate();
		player.Name = id.ToString();
		player.Position = new Vector2(140, 187); // Position the player at the spawnpoint. 
		Level.AddChild(player);
		
		if (!Level.HasNode("1")) // Host = Peer ID 1
		{
			var host = (Player)scene.Instantiate();
			host.Name = "1";
			Level.AddChild(host);
		}
	}
	
}
