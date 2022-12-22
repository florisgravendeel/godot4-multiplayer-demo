extends Node2D

var multiplayer_peer = ENetMultiplayerPeer.new()

@onready var level = get_node("%Level")
@onready var menu = get_node("%MainMenu")
@onready var inputfield = get_parent().get_node("MainMenu/Redwall/IPAddress")

var port = 25541

func _ready():
	level.hide()
	menu.position = Vector2.ZERO

func _on_join_pressed():
	multiplayer_peer.create_client(inputfield.text, port)
	multiplayer.multiplayer_peer = multiplayer_peer
	hide_menu()


func _on_host_pressed():
	multiplayer_peer.create_server(port)
	multiplayer.multiplayer_peer = multiplayer_peer
	multiplayer_peer.peer_connected.connect(func(id): add_player_character(id))
	hide_menu()
	add_player_character()


func add_player_character(id=1):
	var character = preload("res://Player/player.tscn").instantiate()
	character.name = str(id)
	add_child(character)
	

func hide_menu():
	menu.hide()
	menu.position = Vector2(3000, 3000);
	show()
