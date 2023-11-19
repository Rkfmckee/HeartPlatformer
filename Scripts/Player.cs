using Godot;

public partial class Player : CharacterBody2D
{
	#region Fields

	#region Constants

	private const float speed = 100;
	private const float acceleration = 600;
	private const float jumpVelocity = -300;

	#endregion

	#region Nodes

	private AnimatedSprite2D animatedSprite2D;

	#endregion

	private bool canDoubleJump;

	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	#endregion

	public override void _Ready()
	{
		base._Ready();

		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		canDoubleJump = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;
		var direction = Input.GetAxis("ui_left", "ui_right");
		var movingTowards = direction != 0 ? direction * speed : 0f;

		ApplyGravityAndJumping(ref velocity, delta);

		velocity.X = Mathf.MoveToward(Velocity.X, movingTowards, acceleration * (float)delta);
		Velocity = velocity;

		UpdateAnimations(direction);
		MoveAndSlide();
	}

	private void ApplyGravityAndJumping(ref Vector2 velocity, double delta)
	{
		if (!IsOnFloor())
		{
			if (Input.IsActionJustPressed("ui_up") && canDoubleJump)
			{
				velocity.Y = jumpVelocity * 0.75f;
				canDoubleJump = false;
			}

			velocity.Y += gravity * (float)delta;
			return;
		}

		canDoubleJump = true;
		if (Input.IsActionJustPressed("ui_up"))
		{
			velocity.Y = jumpVelocity;
		}
	}

	private void UpdateAnimations(float direction)
	{
		animatedSprite2D.FlipH = direction < 0;

		if (!IsOnFloor())
		{
			animatedSprite2D.Play("Jump");
			return;
		}

		if (direction != 0)
		{
			animatedSprite2D.Play("Run");
			return;
		}

		animatedSprite2D.Play("Idle");
	}
}
