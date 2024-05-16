using Godot;
using GodotPlugins.Game;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public partial class mapUI : Node
{
	private Global global;
	private Dictionary<string, string> mapButtonToScenePath = new Dictionary<string, string>();
	[Export]
	private TextureRect[] thumbnails = new TextureRect[3];
	
	private LineEdit textfield;
	public override void _Ready()
	{
		textfield = GetNode<LineEdit>("Margin/HBoxContainer/MarginContainer/TextEdit");
		global = GetNode<Global>("/root/Global");
		mapButtonToScenePath.Add("Map1","res://scenes/maps/map1.tscn");
		mapButtonToScenePath.Add("Map2","res://scenes/maps/map2.tscn");
		mapButtonToScenePath.Add("Map3","res://scenes/maps/map3.tscn");

        GenerateThumbnail("map1", thumbnails[0]);
		GenerateThumbnail("map2", thumbnails[1]);
		GenerateThumbnail("map3", thumbnails[2]);
	}

	public void GenerateThumbnail(string mapName, TextureRect textureRect)
    {
        textureRect.Texture = GD.Load<Texture2D>("res://assets/thumbnails/" + mapName + ".png");
    }

	public void _on_map_1_pressed(){
		global.currentMap = mapButtonToScenePath["Map1"];
		global.roundsToWin = textfield.Text.ToInt();
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}
	public void _on_map_2_pressed(){
		global.currentMap = mapButtonToScenePath["Map2"];
		global.roundsToWin = textfield.Text.ToInt();
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}
	public void _on_map_3_pressed(){
		global.currentMap = mapButtonToScenePath["Map3"];
		global.roundsToWin = textfield.Text.ToInt();
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}

	/// <summary>
    /// Handles the event when the customize control button is pressed. Changes the scene to the player customization interface.
    /// </summary>
	public void _on_customize_control_pressed(){
		GetTree().ChangeSceneToFile("res://UserInterface/playerCustomize.tscn");
	}
	public void _on_go_back_to_home_pressed(){
		
		GetTree().ChangeSceneToFile("res://UserInterface/ui.tscn");
	
	}
}
