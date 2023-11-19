using Godot;

[GlobalClass]
public partial class PlayerMovementData : Resource
{
	[Export]
	public float Speed { get; set; } = 100;

	[Export]
	public float Acceleration { get; set; } = 600;

	[Export]
	public float JumpVelocity { get; set; } = -300;
}
