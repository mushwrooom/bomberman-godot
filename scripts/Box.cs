using Godot;
using System;

/// <summary>
/// Represents a box object in the game, which can contain a power-up. Inherits from GameObject.
/// </summary>
public partial class Box : StaticBody3D
{
	private Generic_PowerUp powerup;
	public bool PlacedObstacle = false;

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
		if (rng.NextDouble() < 0.5 && !PlacedObstacle)
		{
			Powerup powerupInstance = powerUpScene.Instantiate<Powerup>();
			switch (rng.Next(0, 7)) 
			{
				case 0:
					powerupInstance.SetType(new Number_PowerUp());
					break;
				case 1:
					powerupInstance.SetType(new Blast_PowerUp());
					break;
				case 2:
					powerupInstance.SetType(new Roller_PowerUp());
					break;
				case 3:
					powerupInstance.SetType(new Invincibility_PowerUp());
					break;
				case 4:
					powerupInstance.SetType(new Ghost_PowerUp());
					break;
				case 5:
					powerupInstance.SetType(new Detonator_PowerUp());
					break;
				case 6:
					powerupInstance.SetType(new Obstacle_PowerUp());
					break;
			}
			powerupInstance.Position = Position;
        	GetParent().AddChild(powerupInstance);
		}
		QueueFree();
	}

}
