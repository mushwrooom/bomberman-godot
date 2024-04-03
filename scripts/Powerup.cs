using Godot;
using System;

public partial class Powerup : GameObject {
    private Generic_PowerUp type;


    // Method to get the type of the power-up
    public Generic_PowerUp GetType() {
        return type;
    }

   
    public override void Destroy() {
        QueueFree(); 
    }
}
