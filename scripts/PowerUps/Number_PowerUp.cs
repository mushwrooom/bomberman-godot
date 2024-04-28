using Godot;
using System;
public partial class Number_PowerUp: Generic_PowerUp {
    //increase player's bombcount by 1
    public override void ApplyEffect(Player player) {
        player.AddBombLimit(); 
    }
}
