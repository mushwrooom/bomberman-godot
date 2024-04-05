using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the user interface related to map selection and navigation within the game.
/// </summary>

public partial class mapUI : Control
{

	//private Main _main; // reeference to main


	private Main main;

	/// <summary>
    /// Maps button names to their respective scene paths for map selection.
    /// </summary>
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

	/// <summary>
    /// Connects a button signal for map selection.
    /// </summary>
    /// <param name="buttonName">The name of the button to connect.</param>

	private void ConnectButtonSignal(string buttonName)
	{
    	var button = GetNodeOrNull<Button>(buttonName);
        if (button != null)
        {
            //button.Connect("pressed", this, nameof(_on_Map_selected), new Godot.Collections.Array { buttonName });
        }
        else
        {
            GD.PrintErr($"Button {buttonName} not found.");
        }
	}


	 // This method is called when a map selection button is pressed.


	 /// <summary>
    /// Handles the event when a map selection button is pressed. Changes the current map in the game.
    /// </summary>
    /// <param name="buttonName">The name of the button that was pressed.</param>

     private void _on_Map_selected(string buttonName)
    {
        if (mapButtonToScenePath.TryGetValue(buttonName, out var scenePath))
        {
            // Use the scene path to change the map in the Main class
            Main.currentMap = scenePath;
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
