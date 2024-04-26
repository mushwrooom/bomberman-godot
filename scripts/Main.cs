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
	public static int playerCount = 2;
	public Dictionary<Control, Key>[] playerControls = new Dictionary<Control, Key>[playerCount];
	public int[] scores = new int[playerCount];

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		currentMap = global.currentMap;
		playerControls = global.playerControls;
		StartGame();
	}
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
		PackedScene res = GD.Load<PackedScene>("res://scenes/characters.tscn");
		Characters = res.Instantiate<Node3D>();
		Characters.Position = Vector3.Zero;
		map.AddChild(Characters);
	}

	public async void EndGame()
	{
		//Wait for some time for draw condition
		await Task.Delay(TimeSpan.FromMilliseconds(3000));

		//Then check if it is a draw or a win
		if (IsDraw())
		{
			GD.Print("Draw!");
			GetTree().ChangeSceneToFile("res://UserInterface/EndScreen.tscn");
		}
		else
		{
			GD.Print("Player " + map.GetPlayers()[0].GetPlayerID() + " wins!");
			GetTree().ChangeSceneToFile("res://UserInterface/EndScreen.tscn");
		}
	}

	private bool IsEnd()
	{
		return map.GetPlayers().Count == 1;
	}
	private bool IsDraw()
	{
		return map.GetPlayers().Count == 0;
	}
}
