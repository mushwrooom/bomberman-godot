using Godot;
using System;

public partial class Roller_PowerUp : Generic_PowerUp
{
    private int speed = 7;
    // change & increase the speed of the player for the rest duration of the game 
    public override void ApplyEffect(Player player)
    {
        player.speed = speed;
    }
}
