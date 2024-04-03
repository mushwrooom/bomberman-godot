using Godot;
using System;

public partial class Box : GameObject {
	private Generic_PowerUp powerup;

	public Generic_PowerUp getPowerup() {
		return powerup;
	}

	public void setPowerup(Generic_PowerUp newPowerup) {
		powerup = newPowerup;
	}

	public override void Destroy() {
		QueueFree();
		//The node will be removed from the scene.
	}
}
