using Godot;
using System;
using System.Collections.Generic;


/// <summary>
/// Represents a single tile in the game world. It can contain a game object and has a specified world position.
/// </summary>
public partial class Tile : StaticBody3D {
	//public TileType Type { get; private set; }

	 /// <summary>
    /// Gets the content of the tile. This property holds a reference to the GameObject that is placed on this tile.
    /// </summary>
	public StaticBody3D Content { get; private set; } 

	/// <summary>
    /// Gets or sets the world position of the tile. This is the position of the tile in the game world's 3D space.
    /// </summary>
	public Vector3 WorldPosition { get; set; }


	public void SetContent(StaticBody3D content) {
		Content = content;
	}
}
