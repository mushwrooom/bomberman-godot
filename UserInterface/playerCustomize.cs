using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages player customization including input remapping.
/// </summary>
public partial class playerCustomize : Control
{
    [Export]
    private VBoxContainer[] actionList = new VBoxContainer[2];
    private Global global;
    /// <summary>
    /// Represents the current state of a player's control customization, including the selected button, key mapping, scheme, and the id of a player
    /// </summary>
    private struct Current
    {
        public Button b;
        public Key k;
        public ControlScheme sc;
        public int id;
    }
    private Current cur;

    /// <summary>
    /// Sets up initial button names and attaches button press event handlers for all action buttons.
    /// </summary>
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
        UpdateButtonNames(0);
        UpdateButtonNames(1);

        foreach (Node child in actionList[0].GetChildren())
        {
            if (child is Button b)
                b.Pressed += () => _on_ButtonPressed(b, 0);
        }

        foreach (Node child in actionList[1].GetChildren())
        {
            if (child is Button b)
                b.Pressed += () => _on_ButtonPressed(b, 1);
        }
    }

    /// <summary>
    /// Handles button press events by updating the struct cur.
    /// </summary>
    /// <param name="b">The button that was pressed.</param>
    /// <param name="id">The player ID (0 or 1) corresponding to the button.</param>
    private void _on_ButtonPressed(Button b, int id)
    {
        cur.b = b;
        cur.sc = (ControlScheme)Enum.Parse(typeof(ControlScheme), b.Text);
        cur.id = id;
        GD.Print(b.Text);
        GD.Print("Scheme: " + cur.sc.ToString());
    }

    /// <summary>
    /// Handles the save button press event to save current settings and return to the main UI scene.
    /// </summary>
    private void _on_save_pressed()
    {
        GetTree().ChangeSceneToFile("res://UserInterface/ui.tscn");
        UpdateButtonNames(0);
        UpdateButtonNames(1);
    }

    /// <summary>
    /// Processes key input events for updating player controls in global and labels corresponding to the new key mappings.
    /// </summary>
    /// <param name="@event">The input event.</param>
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
            if (eventKey.Pressed)
            {
                GD.Print(eventKey.Keycode);
                cur.k = eventKey.Keycode;
            }

        //update player controls in global 
        global.playerControls[cur.id][cur.sc] = cur.k;

        UpdateButtonNames(0);
        UpdateButtonNames(1);
    }

    /// <summary>
    /// Updates the names and labels of the buttons to reflect the current control schemes and their assigned keys.
    /// </summary>
    /// <param name="playerID">The ID of the player (0 or 1) whose button names are to be updated.</param>
    private void UpdateButtonNames(int playerID)
    {
        int i = 0;
        foreach (Node child in actionList[playerID].GetChildren())
        {
            if (child is Button button)
            {
                //assign names of the buttons to the enum values 
                ControlScheme scheme = (ControlScheme)i; // Cast i to ControlScheme
                button.Text = scheme.ToString(); // Convert the enum value to string
                i++;

                // Update the label for the corresponding key control
                var label = button.GetChild(0).GetChild(0).GetChild(1) as Label;
                if (global.playerControls[playerID].TryGetValue(scheme, out Key k))
                    label.Text = k.ToString();
            }
        }
    }
}
