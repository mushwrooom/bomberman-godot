using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the user interface related to map selection and navigation within the game.
/// </summary>

public partial class mapUI : Control
{
	private Global global;

	/// <summary>
    /// Maps button names to their respective scene paths for map selection.
    /// </summary>
	private Dictionary<string, string> mapButtonToScenePath = new Dictionary<string, string>();
	

	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		mapButtonToScenePath.Add("Map1","res://scenes/maps/map.tscn");
		mapButtonToScenePath.Add("Map2","res://scenes/maps/map2.tscn");
		mapButtonToScenePath.Add("Map3","res://scenes/maps/map3.tscn");
	}

	public void _on_map_1_pressed(){
		global.currentMap = mapButtonToScenePath["Map1"];
	}
	public void _on_map_2_pressed(){
		global.currentMap = mapButtonToScenePath["Map2"];
	}
	public void _on_map_3_pressed(){
		global.currentMap = mapButtonToScenePath["Map3"];
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
