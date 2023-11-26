using System.Threading.Tasks;
using Godot;

public partial class World : Node2D
{
	#region References

	[Export]
	private PackedScene nextLevel;

	private Events events;
	private Label currentHearts;
	private ColorRect levelCompletedScreen;
	private LevelTransition levelTransition;

	#endregion

	#region Fields

	private int heartsCollected;

	#endregion

	#region Events

	public override void _Ready()
	{
		events = GetNode<Events>("/root/Events");
		events.HeartCollected += HeartCollected;

		levelCompletedScreen = GetNode<ColorRect>("CanvasLayer/LevelCompleted");
		levelTransition = GetNode<LevelTransition>("/root/LevelTransition");

		var heartCounter = GetNode<BoxContainer>("CanvasLayer/HeartCounter");
		var totalHearts = heartCounter.GetNode<Label>("TotalHearts");
		currentHearts = heartCounter.GetNode<Label>("CurrentHearts");
		totalHearts.Text = GetTree().GetNodesInGroup("Hearts").Count.ToString();

		heartsCollected = 0;
	}

	#endregion

	#region Methods

	private async void HeartCollected()
	{
		var heartsRemaining = GetTree().GetNodesInGroup("Hearts").Count - 1;
		heartsCollected++;
		currentHearts.Text = heartsCollected.ToString();

		if (heartsRemaining <= 0)
		{
			levelCompletedScreen.Show();
			GetTree().Paused = true;
			await WaitForSeconds(1);

			if (!nextLevel.IsValid()) return;

			await levelTransition.FadeToBlack();

			GetTree().Paused = false;
			GetTree().ChangeSceneToPacked(nextLevel);

			await levelTransition.FadeFromBlack();
		}
	}

	private async Task WaitForSeconds(float seconds)
	{
		var timer = GetTree().CreateTimer(seconds);
		await ToSignal(timer, Timer.SignalName.Timeout);
	}

	#endregion
}
