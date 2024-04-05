using Godot;
using System;

/// <summary>
/// Represents a power-up in the game. Inherits from GameObject and adds functionality specific to power-ups, such as a type property.
/// </summary>
public partial class Powerup : GameObject {
    private Generic_PowerUp type;

    public Generic_PowerUp GetType() {
        return type;
    }

   
    public override void Destroy() {
        QueueFree(); 
    }
}
