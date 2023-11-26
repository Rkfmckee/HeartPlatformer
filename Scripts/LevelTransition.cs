using System.Collections;
using System.Threading.Tasks;
using Godot;

public partial class LevelTransition : CanvasLayer
{
	#region References

	private AnimationPlayer animationPlayer;

	#endregion

	#region Events

	public override void _Ready()
	{
		base._Ready();

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	#endregion

	#region Methods

	public async Task FadeToBlack()
	{
		animationPlayer.Play("FadeToBlack");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}

	public async Task FadeFromBlack()
	{
		animationPlayer.Play("FadeFromBlack");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}

	#endregion
}
