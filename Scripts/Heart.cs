using Godot;

public partial class Heart : Area2D
{
	#region References

	private Events events;
	private Label currentHearts;

	#endregion

	#region Fields

	private int heartsCollected;

	#endregion

	#region Events

	public override void _Ready()
	{
		base._Ready();

		events = GetNode<Events>("/root/Events");
		currentHearts = GetNode<Label>("/root/World/CanvasLayer/HeartCounter/CurrentHearts");

		BodyEntered += (Node2D body) => Collected();
	}

	#endregion

	#region Methods

	private void Collected()
	{
		QueueFree();

		events.EmitSignal(Events.SignalName.HeartCollected);
	}

	#endregion
}
