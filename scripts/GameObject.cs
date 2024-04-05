using Godot;
using System;

/// <summary>
/// Serves as the base class for all game objects within the game world that have a physical presence, inheriting from Godot's StaticBody3D.
/// </summary>
public abstract partial class GameObject : StaticBody3D
{
    public abstract void Destroy();
}
