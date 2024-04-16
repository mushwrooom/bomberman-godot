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

	public void SetContent(StaticBody3D content)
	{
		this.Content = content;
	}

	/// <summary>
	/// Find the content on top of it and set it as its Content property
	/// </summary>
	public void FindContent()
	{
		var ray = GetNode<RayCast3D>("Ray");
		ray.ForceRaycastUpdate();
		if (!ray.IsColliding())
		{
			Content = null;
			return;
		}

		var result = ray.GetCollider();


		Content = result as StaticBody3D;
	}
}