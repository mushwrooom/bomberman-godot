using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public partial class Main : Node
{
	public Global global;
	public Map map;
	public Node3D Characters;

	public override void _Ready()
	{
		GD.Print("Main.cs is ready");
		global = GetNode<Global>("/root/Global");
		StartGame();
	}

	/// <summary>
	/// Regularly checks game end conditions during the game loop. 
	/// </summary>
	/// <param name="delta">The frame time elapsed since the last call.</param>
	public override void _Process(double delta)
	{
		if (IsEnd())
		{
			//If the end condition is satisfied, call EndGame() and set this recurring _Process function call to false.
			EndGame();
			SetProcess(false);
		}
	}

	/// <summary>
	/// Sets up the game environment, map, players, and HUD elements.
	/// </summary>
	public void StartGame()
	{
		PackedScene res = GD.Load<PackedScene>(global.currentMap);
		map = res.Instantiate<Map>();
		map.Position = Vector3.Zero;
		AddChild(map);
		map.CreateField(10, 10);

		SetupCharacters();

		// Setup HUD
		PackedScene hudRes = GD.Load<PackedScene>("res://UserInterface/HUD.tscn");
		HUD hud = hudRes.Instantiate<HUD>();
		GetNode("Misc").AddChild(hud);
		hud.player1 = map.GetPlayers()[0];
		hud.player2 = map.GetPlayers()[1];
		hud.timer = map.timer;
	}

	/// <summary>
	/// Loads player characters on the map and assigns the control schemes based on global settings
	/// </summary>
	public void SetupCharacters()
	{
		// Instantiate the characters
		PackedScene res = GD.Load<PackedScene>("res://scenes/characters.tscn");
		Characters = res.Instantiate<Node3D>();
		Characters.Position = Vector3.Zero;
		map.AddChild(Characters);

		//setup players's control schemes from global 
		map.GetPlayers()[0].SetControlSchemes(global.playerControls[0]);
		map.GetPlayers()[1].SetControlSchemes(global.playerControls[1]);
		
	}

	/// <summary>
	/// Waits for 3 seconds before checking if the game ended in a draw or a win, then updates scores and changes to the end screen scene.
	/// </summary>
	public async void EndGame()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(3000));

		//After 3 seconds a player died, check if it is a draw or a win
		if (IsDraw())
		{
			for(int i = 0; i < global.scores.Count(); i++)
				global.scores[i]++;
			
			global.endMessage = "Draw!";
		}
		else
		{
			global.scores[map.GetPlayers()[0].GetPlayerID()-1]++;
			global.endMessage = "Player " + map.GetPlayers()[0].GetPlayerID() + " wins!";
		}
		GetTree().ChangeSceneToFile("res://UserInterface/EndScreen.tscn");
	}

	/// <summary>
	/// Checks if the game should end based on the number of remaining players.
	/// </summary>
	private bool IsEnd()
	{
		return map.GetPlayers().Count == 1;
	}

    /// <summary>
	/// Determines if the game ended in a draw.
	/// </summary>
	private bool IsDraw()
	{
		return map.GetPlayers().Count == 0;
	}
}
