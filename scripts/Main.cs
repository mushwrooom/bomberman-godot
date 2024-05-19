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
    /// This function is called after a player dies. It will wait for 3 seconds (same as bomb's exploding time) then check if it's a draw or a win state. 
	/// If a player wins, the corresponding score will be incremented. Either it's a draw or a win, it shows the End scene and players will have a chance to restart the game. 
    /// </summary>
	/// 


	public async void EndGame()
    {
        GD.Print("EndGame called");

		int winnerID = map.GetPlayers()[0].GetPlayerID();
        // Wait for some time for draw condition
        await Task.Delay(TimeSpan.FromMilliseconds(3000));

        // Then check if it is a draw or a win
        if (IsDraw())
        {
            for (int i = 0; i < global.scores.Length; i++)
            {
                global.scores[i]++;
            }
            global.endMessage = "Draw!";
            GD.Print("It's a draw. Scores updated to: " + string.Join(", ", global.scores));
        }
        else
        {
            global.scores[winnerID - 1]++;
			global.endMessage = "Player " + winnerID + " wins. Scores updated to: " + string.Join(", ", global.scores);
        }

        // Check if any player has reached the required rounds to win
        if (global.scores.Any(score => score >= global.roundsToWin))
        {
			global.endMessage = "Game over. Player " + winnerID + " wins!";
            GD.Print("Game over. Final scores: " + string.Join(", ", global.scores));
        }
        else
        {
            global.currentRound++; 
            GD.Print("Starting next round. Current round: " + global.currentRound);
            StartGame();
        }
		GetTree().ChangeSceneToFile("res://UserInterface/EndScreen.tscn");
    }

	/// <summary>
	/// Checks if the game should end based on the number of remaining players.
	/// </summary>
	public bool IsEnd()
	{
		return map.GetPlayers().Count <= 1;
	}

    /// <summary>
	/// Determines if the game ended in a draw.
	/// </summary>
	public bool IsDraw()
	{
		return map.GetPlayers().Count == 0;
	}
}
