using Godot;
using System;
using System.Collections.Generic;


/// <summary>
/// Manages player customization including input remapping.
/// </summary>


public partial class playerCustomize : Control
{
	// Called when the node enters the scene tree for the first time.


	/// <summary>
    /// Represents the packed scene for an input button.
    /// </summary>
    private PackedScene inputButtonScene = (PackedScene)ResourceLoader.Load("res://UserInterface/input_button.tscn");

    // 'onready' keyword is not needed in C#, but you can initialize your variables here

    /// <summary>
    /// Container for listing actions that can be remapped.
    /// </summary>
    private PanelContainer actionList;

    /// <summary>
    /// Flag indicating whether input remapping is currently in progress.
    /// </summary>
    private bool isRemapping = false;

    /// <summary>
    /// Stores the current action being remapped.
    /// </summary>
    private object actionToRemap = null; // object type may vary, use the specific type you need


    /// <summary>
    /// Reference to the button currently being used for remapping.
    /// </summary>
    private Button remappingButton = null;

    /// <summary>
    /// A dictionary mapping input actions to their display names.
    /// </summary>
	private Dictionary<string, string> inputActions = new Dictionary<string, string>
	{
		{ "move_up", "Move up" },
        { "move_left", "Move Left" },
        { "move_down", "Move down" },
        { "move_right", "Move Right" },
        { "shoot", "Shoot" },
        { "interact", "Interact" }
	}; 



	public override void _Ready()
	{
        actionList = GetNode<PanelContainer>("PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/ActionList");
		//creataction function
		CreateActionList();
		
	}


    /// <summary>
    /// Creates the list of actions that can be remapped by the player. This method dynamically creates buttons for each action
    /// and assigns the appropriate labels and input events.
    /// </summary>
	 private void CreateActionList()
    {
        InputMap.LoadFromProjectSettings();

        foreach (var item in actionList.GetChildren())
        {
            if (item is Node nodeItem)
            {
                nodeItem.QueueFree(); // clearing existing items
            }
        }

        foreach (var action in inputActions.Keys)
        {
            var button = (Button)inputButtonScene.Instantiate();
            var actionLabel = (Label)button.FindChild("LabelAction");
            var inputLabel = (Label)button.FindChild("LabelInput");

            actionLabel.Text = action;

            var events = InputMap.ActionGetEvents(action);

            if (events.Count > 0)
            {
                if (events[0] is InputEvent inputEvent)
                {
                    inputLabel.Text = inputEvent.AsText();
                }
            }
            else
            {
                inputLabel.Text = "";
            }

            actionList.AddChild(button);
        }
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	


	
}
