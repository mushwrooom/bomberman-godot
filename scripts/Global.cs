using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
	public string currentMap = "res://scenes/maps/map.tscn";
	public Dictionary<Control, Key>[] playerControls = new Dictionary<Control, Key>[2];
}
