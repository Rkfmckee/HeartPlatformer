using Godot;

public partial class StartMenu : CenterContainer
{
	#region References

	private Button startButton;
	private Button quitButton;
	private LevelTransition levelTransition;

	#endregion

	#region Events

	public override void _Ready()
	{
		base._Ready();

		RenderingServer.SetDefaultClearColor(new Color(0, 0, 0));

		startButton = GetNode<Button>("%StartGameButton");
		startButton.Pressed += () => StartButtonPressed();
		startButton.GrabFocus();

		quitButton = GetNode<Button>("%QuitButton");
		quitButton.Pressed += () => QuitButtonPressed();

		levelTransition = GetNode<LevelTransition>("/root/LevelTransition");
	}

	#endregion

	#region Methods

	private async void StartButtonPressed()
	{
		await levelTransition.FadeToBlack();
		GetTree().ChangeSceneToFile("res://Scenes/Level1.tscn");
		await levelTransition.FadeFromBlack();
	}

	private void QuitButtonPressed()
	{
		GetTree().Quit();
	}

	#endregion
}
