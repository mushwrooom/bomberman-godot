using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

public partial class mapUI : Control
{

	//private Main _main; // reeference to main


	private Main main;
	private Dictionary<string, string> mapButtonToScenePath = new Dictionary<string, string>();
	

	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GetNode("Map1").Connect("pressed", this, nameof(_on_Map_selected), new Godot.Collections.Array { 1 });
        // GetNode("Map2").Connect("pressed", this, nameof(_on_Map_selected), new Godot.Collections.Array { 2 });
        // GetNode("Map3").Connect("pressed", this, nameof(_on_Map_selected), new Godot.Collections.Array { 3 });
		// ConnectButtonSignal("Map1", 1);
    	// ConnectButtonSignal("Map2", 2);
    	// ConnectButtonSignal("Map3", 3);


		// Node a = GD.Load("path_for_3maps");
		mapButtonToScenePath.Add("Map1","res:pathtomap1");
		mapButtonToScenePath.Add("Map2","res:pathtomap2");
		mapButtonToScenePath.Add("Map3","res:pathtomap3");


		main = GetNode<Main>("/root/Main");


		foreach(var mapButton in mapButtonToScenePath.Keys){
			ConnectButtonSignal(mapButton);
		}
		


	}

	private void ConnectButtonSignal(string buttonName)
	{
    	var button = GetNodeOrNull<Button>(buttonName);
        if (button != null)
        {
            button.Connect("pressed", this, nameof(_on_Map_selected), new Godot.Collections.Array { buttonName });
        }
        else
        {
            GD.PrintErr($"Button {buttonName} not found.");
        }
	}


	 // This method is called when a map selection button is pressed.


	 //mainii current mapiig solih

     private void _on_Map_selected(string buttonName)
    {
        if (mapButtonToScenePath.TryGetValue(buttonName, out var scenePath))
        {
            // Use the scene path to change the map in the Main class
            main.CurrentMapScenePath = scenePath;
        }
        else
        {
            GD.PrintErr($"Scene path for button {buttonName} not found.");
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_customize_control_pressed(){
		GetTree().ChangeSceneToFile("res://UserInterface/playerCustomize.tscn");
	
	}
	public void _on_go_back_to_home_pressed(){
		
		GetTree().ChangeSceneToFile("res://UserInterface/ui.tscn");
	
	}


}