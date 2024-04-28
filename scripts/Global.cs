using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
	public string currentMap = "res://scenes/maps/map.tscn";
	public string endMessage = "Game Over!";
	public int[] scores = new int[2];
	public List<Dictionary<ControlScheme, Key>> playerControls = new List<Dictionary<ControlScheme, Key>>
		//Default Control
		{
			new() {
				{ ControlScheme.MOVE_UP, Key.W },
				{ ControlScheme.MOVE_DOWN, Key.S },
				{ ControlScheme.MOVE_LEFT, Key.A },
				{ ControlScheme.MOVE_RIGHT, Key.D },
				{ ControlScheme.PLACE_BOMB, Key.Space },
				{ ControlScheme.PLACE_OBSTACLE, Key.B }
			},
			new() {
				{ ControlScheme.MOVE_UP, Key.Up },
				{ ControlScheme.MOVE_DOWN, Key.Down },
				{ ControlScheme.MOVE_LEFT, Key.Left },
				{ ControlScheme.MOVE_RIGHT, Key.Right },
				{ ControlScheme.PLACE_BOMB, Key.Enter },
				{ ControlScheme.PLACE_OBSTACLE, Key.O }
			}
		};
}
