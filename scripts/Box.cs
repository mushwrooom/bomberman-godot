using Godot;
using System;

/// <summary>
/// Represents a box object in the game, which can contain a power-up. Inherits from GameObject.
/// </summary>
public partial class Box : StaticBody3D
{
	private Generic_PowerUp powerup;

	public Generic_PowerUp getPowerup()
	{
		return powerup;
	}

	public void setPowerup(Generic_PowerUp newPowerup)
	{
		powerup = newPowerup;
	}

	private PackedScene powerUpScene;

    public override void _Ready()
    {
		powerUpScene = GD.Load<PackedScene>("res://scenes/PowerUp.tscn");
    }

    public void Destroy()
	{
		var rng = new Random();
		if (rng.NextDouble() < 0.5)
		{ // 50% chance to spawn a power-up
			Powerup powerupInstance = powerUpScene.Instantiate<Powerup>();
			var powerupType = rng.Next(0, 2);
			powerupInstance.SetType(powerupType == 0 ? new Number_PowerUp() : new Blast_PowerUp());
			powerupInstance.Position = Position;
			GetParent().AddChild(powerupInstance);
		}
		QueueFree();
	}
	

}
