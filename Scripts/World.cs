using Godot;

public partial class World : Node2D
{
	#region References

	private CollisionPolygon2D collisionPolygon;
	private Polygon2D polygon;
	private Events events;
	private ColorRect levelCompletedScreen;
	private Label currentHearts;

	#endregion

	#region Fields

	private int heartsCollected;

	#endregion

	#region Events

	public override void _Ready()
	{
		collisionPolygon = GetNode<CollisionPolygon2D>("StaticBody2D/CollisionPolygon2D");
		polygon = collisionPolygon.GetNode<Polygon2D>("Polygon2D");

		polygon.Polygon = collisionPolygon.Polygon;
		RenderingServer.SetDefaultClearColor(new Color(0, 0, 0));

		events = GetNode<Events>("/root/Events");
		events.HeartCollected += HeartCollected;

		levelCompletedScreen = GetNode<ColorRect>("CanvasLayer/LevelCompleted");

		var heartCounter = GetNode<BoxContainer>("CanvasLayer/HeartCounter");
		var totalHearts = heartCounter.GetNode<Label>("TotalHearts");
		currentHearts = heartCounter.GetNode<Label>("CurrentHearts");
		totalHearts.Text = GetTree().GetNodesInGroup("Hearts").Count.ToString();

		heartsCollected = 0;
	}

	#endregion

	#region Methods

	private void HeartCollected()
	{
		var heartsRemaining = GetTree().GetNodesInGroup("Hearts").Count - 1;
		heartsCollected++;
		currentHearts.Text = heartsCollected.ToString();

		if (heartsRemaining <= 0)
		{
			levelCompletedScreen.Show();
			GetTree().Paused = true;
		}
	}

	#endregion
}
