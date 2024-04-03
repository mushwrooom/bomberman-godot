using Godot;
using System;
using System.Collections.Generic;

public partial class playerCustomize : Control
{
	// Called when the node enters the scene tree for the first time.


	// Note: For preload, in C# you would typically load the resource when you need it
    // or you could use a static constructor or a field initializer.
    private PackedScene inputButtonScene = (PackedScene)ResourceLoader.Load("res://UserInterface/input_button.tscn");

    // 'onready' keyword is not needed in C#, but you can initialize your variables here
    private PanelContainer actionList;

    private bool isRemapping = false;
    private object actionToRemap = null; // object type may vary, use the specific type you need
    private Button remappingButton = null;


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
