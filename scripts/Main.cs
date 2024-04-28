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
	public string currentMap = "res://scenes/maps/map.tscn";
	public Map map;
	public Node3D Characters;

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		currentMap = global.currentMap;
		StartGame();
	}

	/// <summary>
    /// Called in each time frame to check if a player is dead. If yes, it calls EndGame to check draw condition and stop calling _Process again. 
    /// </summary>
	public override void _Process(double delta)
	{
		if (IsEnd())
		{
			//If the end condition is satisfied call the EndGame method and set this recurring _Process to false.
			EndGame();
			SetProcess(false);
		}
	}

	public void StartGame()
	{
		PackedScene res = GD.Load<PackedScene>(currentMap);
		map = res.Instantiate<Map>();
		map.Position = Vector3.Zero;
		AddChild(map);
		map.CreateField(10, 10);

		SetupCharacters();

		// Setup HUD
		PackedScene hudRes = GD.Load<PackedScene>("res://UserInterface/HUD.tscn");
		HUD hud = hudRes.Instantiate<HUD>();
		// hud.Position = Vector2.Zero;
		GetNode("Misc").AddChild(hud);
		hud.player1 = map.GetPlayers()[0];
		hud.player2 = map.GetPlayers()[1];
		hud.timer = map.timer;
	}

	public void SetupCharacters()
	{
		// Instantiate the characters
		PackedScene res = GD.Load<PackedScene>("res://scenes/characters.tscn");
		Characters = res.Instantiate<Node3D>();
		Characters.Position = Vector3.Zero;
		map.AddChild(Characters);
	}

    /// <summary>
    /// This function is called after a player dies. It will wait for 3 seconds (same as bomb's exploding time) then check if it's a draw or a win state. 
	/// If a player wins, the corresponding score will be incremented. Either it's a draw or a win, it shows the End scene and players will have a chance to restart the game. 
    /// </summary>
	public async void EndGame()
	{
		//Wait for some time for draw condition
		await Task.Delay(TimeSpan.FromMilliseconds(3000));

		//Then check if it is a draw or a win
		if (IsDraw())
		{
			for(int i = 0; i < global.scores.Count(); i++){
				global.scores[i]++;
			}
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
    /// When a player dies, it returns true
    /// </summary>
	private bool IsEnd()
	{
		return map.GetPlayers().Count == 1;
	}

    /// <summary>
    /// If both players are dead, it returns true 
    /// </summary>
	private bool IsDraw()
	{
		return map.GetPlayers().Count == 0;
	}
}
