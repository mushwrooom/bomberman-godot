using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Node
{
	public static String currentMap = "res://scenes/maps/map.tscn";
	public static Map map;
	public static int monsterCount = 3;
	public static int playerCount = 2;
	public static Dictionary<Control,Key>[] playerControls = new Dictionary<Control, Key>[playerCount];
	public static int[] scores = new int[playerCount];

	public override void _Process(double delta)
	{
		if(IsEnd()) EndGame();
	}
	public void StartGame(){
		GetChild(0).QueueFree();
		GetChild(1).QueueFree();
		PackedScene res = GD.Load<PackedScene>(currentMap);
		map = res.Instantiate<Map>();
		AddChild(map);
		map.CreateField(10,10);
		map.SetupPlayers(playerCount,playerControls.ToList());
		map.SetupMonsters(monsterCount);
	}
	private bool IsEnd(){
		bool result = false;

		//

		return result;
	}
	public void EndGame(){
		
	}
}
