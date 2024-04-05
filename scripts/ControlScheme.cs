using Godot;
using System;

/// <summary>
/// Defines the control scheme actions available in the game.
/// </summary>
/// <remarks>
/// This enumeration maps game actions to specific controls. It is used throughout the game to standardize how player inputs are handled, 
/// ensuring that actions such as moving, placing bombs, or obstacles are consistently managed across different parts of the game.
/// </remarks>
public enum ControlScheme{
		MOVE_UP,
		MOVE_DOWN,
		MOVE_LEFT,
		MOVE_RIGHT,
		PLACE_BOMB,
		PLACE_OBSTACLE
	}
