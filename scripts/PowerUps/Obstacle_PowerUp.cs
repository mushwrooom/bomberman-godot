using Godot;
using System;

public partial class Obstacle_PowerUp : Generic_PowerUp
{
	public override void ApplyEffect(Player player) {
        player.ObstacleStock += 3;
    }

	public override void EndEffect(Player player)
    {
        // Default implementation: No operation.
    } 
}
