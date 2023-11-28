using Godot;

public partial class Player : CharacterBody2D
{
	#region References

	[Export] private PlayerMovementData movementData;
	private AnimatedSprite2D animatedSprite2D;
	private Timer coyoteJumpTimer;
	private Timer wallJumpTimer;
	private Area2D hazardDetector;

	#endregion

	#region Fields

	private Vector2 spawnPosition;
	private bool canDoubleJump;
	private Vector2 mostRecentWallNormal;

	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	#endregion

	#region Events

	public override void _Ready()
	{
		base._Ready();

		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		coyoteJumpTimer = GetNode<Timer>("CoyoteJumpTimer");
		wallJumpTimer = GetNode<Timer>("WallJumpTimer");

		hazardDetector = GetNode<Area2D>("HazardDetector");
		hazardDetector.AreaEntered += (Area2D area) => HitHazard(area);

		spawnPosition = GlobalPosition;
		canDoubleJump = true;
		mostRecentWallNormal = Vector2.Zero;
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;
		var direction = Input.GetAxis("ui_left", "ui_right");

		HandleMovement(ref velocity, direction, delta);
		ApplyGravityAndJumping(ref velocity, delta);

		var wasOnFloor = IsOnFloor();
		var wasOnWall = IsOnWallOnly();

		Velocity = velocity;
		MoveAndSlide();

		StartJumpTimers(wasOnFloor, wasOnWall, velocity);
		UpdateAnimations(velocity);
	}

	#endregion

	#region Methods

	private void ApplyGravityAndJumping(ref Vector2 velocity, double delta)
	{
		if (IsOnFloor() || coyoteJumpTimer.TimeLeft > 0)
		{
			canDoubleJump = true;

			if (Input.IsActionJustPressed("ui_up"))
			{
				velocity.Y = movementData.JumpVelocity;
			}
		}

		if (!IsOnFloor())
		{
			var hasWallJumped = HandleWallJumping(ref velocity);
			var shouldDoubleJump = Input.IsActionJustPressed("ui_up") && canDoubleJump && !hasWallJumped;

			if (shouldDoubleJump)
			{
				velocity.Y = movementData.JumpVelocity * 0.75f;
				canDoubleJump = false;
			}

			velocity.Y += gravity * (float)delta;
		}
	}

	private bool HandleWallJumping(ref Vector2 velocity)
	{
		if (!IsOnWallOnly() && wallJumpTimer.TimeLeft <= 0) return false;

		// Always store the normal of the most recent wall touched,
		// in case we wall jumped using the wallJumpTimer
		if (IsOnWallOnly()) mostRecentWallNormal = GetWallNormal();

		if (Input.IsActionJustPressed("ui_up"))
		{
			velocity.X = mostRecentWallNormal.X * movementData.Speed;
			velocity.Y = movementData.JumpVelocity;
			return true;
		}

		return false;
	}

	private void HandleMovement(ref Vector2 velocity, float direction, double delta)
	{
		var movingTowards = direction != 0 ? direction * movementData.Speed : 0f;
		var acceleration = IsOnFloor() ? movementData.GroundAcceleration : movementData.AirAcceleration;

		velocity.X = Mathf.MoveToward(Velocity.X, movingTowards, acceleration * (float)delta);
	}

	private void UpdateAnimations(Vector2 velocity)
	{
		animatedSprite2D.FlipH = velocity.X < 0;

		if (!IsOnFloor())
		{
			animatedSprite2D.Play("Jump");
			return;
		}

		if (velocity.X != 0)
		{
			animatedSprite2D.Play("Run");
			return;
		}

		animatedSprite2D.Play("Idle");
	}

	private void StartJumpTimers(bool wasOnFloor, bool wasOnWall, Vector2 velocity)
	{
		var justLeftLedge = wasOnFloor && !IsOnFloor() && velocity.Y >= 0;
		if (justLeftLedge) coyoteJumpTimer.Start();

		var justLeftWall = wasOnWall && !IsOnWall();
		if (justLeftWall) wallJumpTimer.Start();
	}

	private void HitHazard(Area2D hazard)
	{
		GlobalPosition = spawnPosition;
		Velocity = Vector2.Zero;
	}

	#endregion
}
