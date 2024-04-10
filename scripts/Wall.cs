using Godot;
using System;

/// <summary>
/// Represents a wall object within the game world. Inherits from GameObject.
/// </summary>
public partial class Wall : StaticBody3D {
    
    /// <summary>
    /// Gets or sets a value indicating whether the wall can be passed through. 
    /// </summary>
    public bool Passable { get; set; }


    /// <summary>
    /// Destroys the wall object, removing it from the game world.
    /// </summary>
    
}
