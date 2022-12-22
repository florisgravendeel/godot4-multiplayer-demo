extends CharacterBody2D

const Speed = 330.0;
const JumpVelocity = -500.0;
const Gravity = 980;
@onready var MaskFrog : AnimatedSprite2D = get_node("MaskFrog")
@onready var synchronizer : MultiplayerState = get_node("State")

func _ready():
	$PlayerTag.text = name
	synchronizer.set_multiplayer_authority(str(name).to_int())
	position = Vector2(140, 187)

func _physics_process(delta):
	if !synchronizer.is_multiplayer_authority():
		position = synchronizer.sync_position
		return
	# Add the gravity
	var movement = velocity;
	if !is_on_floor():
		MaskFrog.animation = "fall"
		movement.y += Gravity * delta;
	
	if Input.is_action_just_pressed("jump") && is_on_floor():
		MaskFrog.animation = "jump"
		movement.y = JumpVelocity
	
	var direction = Input.get_vector("left", "right", "jump", "down")
	if direction != Vector2.ZERO:
		MaskFrog.animation = "run"
		movement.x = direction.x * Speed
		MaskFrog.flip_h = bool(direction.x < 0)
	else: 
		MaskFrog.animation = "idle"
		movement.x = move_toward(velocity.x, 0, Speed)
	
	velocity = movement
	move_and_slide()
	synchronizer.sync_position = position
