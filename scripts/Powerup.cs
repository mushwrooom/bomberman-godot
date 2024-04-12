using Godot;
using System;

/// <summary>
/// Represents a power-up in the game. Inherits from GameObject and adds functionality specific to power-ups, such as a type property.
/// </summary>
public partial class Powerup : StaticBody3D {
    private Generic_PowerUp type;

    public Generic_PowerUp GetPowerUpType() {
        return type;
    }

    public void SetType(Generic_PowerUp value)
    {
        type = value;
    }

    private Label label;
    public override void _Ready()
    {
        label = GetNode<Label>("Sprite3D/SubViewport/Label");
        SetLabel(type);
    }

    
    public void SetLabel(Generic_PowerUp powerupType)
    {
        if (powerupType is Blast_PowerUp)
        {
            label.Text = "Blast Range";
        }
        else if (powerupType is Number_PowerUp)
        {
            label.Text = "Bomb Count";
        }
    }

}

