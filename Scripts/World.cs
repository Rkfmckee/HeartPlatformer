using Godot;

public partial class World : Node2D
{
	private CollisionPolygon2D collisionPolygon;
	private Polygon2D polygon;

	public override void _Ready()
	{
		collisionPolygon = GetNode<CollisionPolygon2D>("StaticBody2D/CollisionPolygon2D");
		polygon = collisionPolygon.GetNode<Polygon2D>("Polygon2D");

		polygon.Polygon = collisionPolygon.Polygon;

		RenderingServer.SetDefaultClearColor(new Color(0, 0, 0));
	}
}
