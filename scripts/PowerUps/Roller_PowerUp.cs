using Godot;
using System;

public partial class Roller_PowerUp : Generic_PowerUp
{
    private int speed = 10;
    public override void ApplyEffect(Player player)
    {
        player.speed = speed;
    }
}
