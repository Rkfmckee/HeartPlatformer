using Godot;

public partial class Events : Node
{
	[Signal]
	public delegate void HeartCollectedEventHandler();
}
