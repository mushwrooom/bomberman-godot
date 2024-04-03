using Godot;
using System;

public partial class Wall : GameObject {
    
    public bool Passable { get; set; }

    public override void Destroy() {
        QueueFree(); 
    }
}
