using System;
using Godot;

public partial class VictoryScreen : CenterContainer
{
	#region References

	private Button menuButton;
	private LevelTransition levelTransition;

	#endregion

	#region Signals

	public override void _Ready()
	{
		menuButton = GetNode<Button>("%MenuButton");
		menuButton.Pressed += () => OnMenuButtonPressed();
		menuButton.GrabFocus();

		levelTransition = GetNode<LevelTransition>("/root/LevelTransition");
		levelTransition.FadeFromBlack();
	}

	private void OnMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/StartMenu.tscn");
	}

	#endregion
}
