using Godot;

public static class StringExtensions
{
	public static bool IsValid<T>(this T node) where T : GodotObject
	{
		return node != null
			&& GodotObject.IsInstanceValid(node)
			&& !node.IsQueuedForDeletion();
	}
}