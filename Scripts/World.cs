using System.Threading.Tasks;
using Godot;

public partial class World : Node2D
{
	#region References

	[Export]
	private PackedScene nextLevel;

	private PackedScene currentLevel;
	private Events events;
	private Label currentHearts;
	private LevelCompleted levelCompleted;
	private LevelTransition levelTransition;
	private AnimationPlayer countdownAnimationPlayer;
	private Label levelTimerLabel;

	#endregion

	#region Fields

	private int heartsCollected;
	private float startLevelTimeMsec;

	#endregion

	#region Events

	public override async void _Ready()
	{
		currentLevel = GD.Load(SceneFilePath) as PackedScene;

		events = GetNode<Events>("/root/Events");
		events.HeartCollected += () => HeartCollected();

		levelTransition = GetNode<LevelTransition>("/root/LevelTransition");
		levelTimerLabel = GetNode<Label>("%LevelTimerLabel");
		levelTimerLabel.Text = "0.00";

		levelCompleted = GetNode<LevelCompleted>("CanvasLayer/LevelCompleted");
		levelCompleted.Retry += async () => await GoToLevel(currentLevel);
		levelCompleted.NextLevel += async () => await GoToLevel(nextLevel);

		countdownAnimationPlayer = GetNode<AnimationPlayer>("CountdownAnimationPlayer");

		var heartCounter = GetNode<BoxContainer>("CanvasLayer/HeartCounter");
		var totalHearts = heartCounter.GetNode<Label>("TotalHearts");
		currentHearts = heartCounter.GetNode<Label>("CurrentHearts");
		totalHearts.Text = GetTree().GetNodesInGroup("Hearts").Count.ToString();

		heartsCollected = 0;
		await DoCountdown();
	}

	public override void _Process(double delta)
	{
		var currentTimeSec = (Time.GetTicksMsec() - startLevelTimeMsec) / 1000;
		levelTimerLabel.Text = currentTimeSec.ToString("0.00");
	}

	private async void HeartCollected()
	{
		var heartsRemaining = GetTree().GetNodesInGroup("Hearts").Count - 1;
		heartsCollected++;
		currentHearts.Text = heartsCollected.ToString();

		if (heartsRemaining <= 0)
		{
			if (!nextLevel.IsValid())
			{
				var victoryScreen = GD.Load("res://Scenes/VictoryScreen.tscn") as PackedScene;
				await GoToLevel(victoryScreen);
				return;
			}

			GetTree().Paused = true;
			levelCompleted.ShowScreen();
		}
	}

	#endregion

	#region Methods

	private async Task DoCountdown()
	{
		GetTree().Paused = true;
		levelTransition.FadeFromBlack();

		countdownAnimationPlayer.Play("CountDown");
		await ToSignal(countdownAnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);

		GetTree().Paused = false;
		startLevelTimeMsec = Time.GetTicksMsec();
	}

	private async Task GoToLevel(PackedScene level)
	{
		if (!level.IsValid()) return;

		await levelTransition.FadeToBlack();
		GetTree().ChangeSceneToPacked(level);
	}

	private async Task WaitForSeconds(float seconds)
	{
		var timer = GetTree().CreateTimer(seconds);
		await ToSignal(timer, Timer.SignalName.Timeout);
	}

	#endregion
}
