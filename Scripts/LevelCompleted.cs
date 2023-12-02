using Godot;

public partial class LevelCompleted : ColorRect
{
	#region References

	private Button retryButton;
	private Button nextLevelButton;

	#endregion

	#region Fields

	[Signal] public delegate void RetryEventHandler();
	[Signal] public delegate void NextLevelEventHandler();

	#endregion

	#region Events

	public override void _Ready()
	{
		retryButton = GetNode<Button>("%RetryButton");
		retryButton.Pressed += () => OnRetryButtonPressed();

		nextLevelButton = GetNode<Button>("%NextLevelButton");
		nextLevelButton.Pressed += () => OnNextLevelButtonPressed();
	}

	#endregion

	#region Signals


	private void OnRetryButtonPressed()
	{
		EmitSignal(SignalName.Retry);
	}

	private void OnNextLevelButtonPressed()
	{
		EmitSignal(SignalName.NextLevel);
	}

	#endregion

	#region Methods

	public void ShowScreen()
	{
		Show();
		retryButton.GrabFocus();
	}

	#endregion
}
