using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Multiplayerdemo.World;

public partial class DustParticles : Node2D
{
	public List<CPUParticles2D> Particles { get; set; }
	public Player Player { get; set; }
	private MultiplayerSynchronizer MultiplayerSynchronizer { get; set; }
	
	public override void _Ready()
	{
		Player = GetParent<Player>();
		MultiplayerSynchronizer = Player.GetNode<MultiplayerSynchronizer>("MultiplayerPlayer/MultiplayerSynchronizer");
		Particles = GetChildren().OfType<CPUParticles2D>().ToList();
		SetProcess(MultiplayerSynchronizer.IsMultiplayerAuthority()); // Enable _Process only on the local authority 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var emitParticles = Player.CurrentSkin.Animation.Equals("run") && Player.IsOnFloor();
		Rpc(nameof(UpdateParticles), emitParticles);
	}

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void UpdateParticles(bool emitting)
	{
		Particles.ForEach(i => i.Emitting = emitting);
	}
}
