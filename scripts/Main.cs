using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This singleton class represents the main game logic and functionality in the Godot game engine.
/// Accessible from anywhere.
/// </summary>
public partial class Main : Node
{
	public String currentMap = "res://scenes/maps/map.tscn";

	/// <summary>
    /// Reference to the instantiated map.
    /// </summary>
	public Map map;
	public int monsterCount = 3;
	public static int playerCount = 2;

    /// <summary>
    /// Array of dictionaries representing player controls.
	/// It is passed as parameter to each players once the StartGame() is called.
    /// </summary>
	public Dictionary<Control,Key>[] playerControls = new Dictionary<Control, Key>[playerCount];
	public int[] scores = new int[playerCount];

	/// <summary>
    /// Process function called on every game frame.
    /// </summary>
	public override void _Process(double delta)
	{
		if(IsEnd()) EndGame();
	}
	/// <summary>
    /// Starts the game by loading the map and invoking setup methods on map.
    /// </summary>
	public void StartGame(){
		//Loads the map scene and stores it as child inside the node
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
		PackedScene res = GD.Load<PackedScene>(currentMap);
		map = res.Instantiate<Map>();
		AddChild(map);
		

		map.CreateField(10,10);
		map.SetupPlayers(playerCount,playerControls.ToList());
		map.SetupMonsters(monsterCount);
	}

	/// <summary>
    /// Checks the game has ended.
    /// </summary>
	/// <returns>True if the game has ended, otherwise false.</returns>
	private bool IsEnd(){
		bool result = false;

		// Logic for determining game end

		return result;
	}

	/// <summary>
    /// Ends the game.
    /// </summary>
	public void EndGame(){
		GetTree().ChangeSceneToFile("res://UserInterface/gameEnd.tscn");
		// Sequence for ending the game and cleanning up.
	}
}
