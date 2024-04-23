using Godot;
using System;
public partial class Blast_PowerUp : Generic_PowerUp {
    public override void ApplyEffect(Player player) {
        player.AddBlastRange(); 
    }
}
