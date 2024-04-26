using Godot;
using System;


public partial class Detonator_PowerUp : Generic_PowerUp
{
    public override void ApplyEffect(Player player)
    {
        player.HasDetonator = true;
    }
}
