using Godot;
using System;
using System.Collections.Generic;



public partial class Tile : Godot.Node {
	//public TileType Type { get; private set; }
	public GameObject Content { get; private set; } 
	public Vector3 WorldPosition { get; set; }


	/*public Tile(TileType type) {
		Type = type;
		//Content = null;
	}*/


	/*public void SetType(TileType type) {
		Type = type;
	}*/

	public void SetContent(GameObject content) {
		Content = content;
	}
}
