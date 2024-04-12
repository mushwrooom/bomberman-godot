using Godot;
using System;
using System.Collections.Generic;


/// <summary>
/// Represents a single tile in the game world. It can contain a game object and has a specified world position.
/// </summary>
public partial class Tile : StaticBody3D
{
	//public TileType Type { get; private set; }

	/// <summary>
	/// Gets the content of the tile. This property holds a reference to the GameObject that is placed on this tile.
	/// </summary>
	public StaticBody3D Content { get; private set; }

	/// <summary>
	/// Gets or sets the world position of the tile. This is the position of the tile in the game world's 3D space.
	/// </summary>
	public Vector3 WorldPosition { get; set; }


	public void SetContent(StaticBody3D content)
	{
		this.Content = content;
	}

	public void FindContent()
	{
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(GlobalPosition + Vector3.Down, Vector3.Up * 100);
		var result = spaceState.IntersectRay(query);
		if (result.Count == 0) Content = null;
		else Content = result["collider"].As<StaticBody3D>();
	}
}
