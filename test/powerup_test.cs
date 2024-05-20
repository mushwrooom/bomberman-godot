using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using System;

[TestSuite]
public class PowerupTests
{
    private Player player;

    [Before]
    public void SetUp()
    {
        player = new Player();
        player._Ready();
    }

    [TestCase]
    public void Test_Blast_PowerUp_ApplyEffect()
    {
        var blastPowerUp = new Blast_PowerUp();
        blastPowerUp.ApplyEffect(player);
        
        AssertThat(player.blastRange).IsGreater(1);
    }

    [TestCase]
    public void Test_Detonator_PowerUp_ApplyEffect()
    {
        var detonatorPowerUp = new Detonator_PowerUp();
        detonatorPowerUp.ApplyEffect(player);
        
        AssertThat(player.HasDetonator).IsTrue();
    }

    [TestCase]
    public void Test_Ghost_PowerUp_ApplyEffect()
    {
        var ghostPowerUp = new Ghost_PowerUp();
        ghostPowerUp.ApplyEffect(player);
        
        AssertThat(player.GhostTimer).IsNotNull();
        AssertThat(player.GhostTimer.IsStopped()).IsFalse();
    }

    [TestCase]
    public void Test_Invincibility_PowerUp_ApplyEffect()
    {
        var invincibilityPowerUp = new Invincibility_PowerUp();
        invincibilityPowerUp.ApplyEffect(player);
        
        AssertThat(player.IsInvincible).IsTrue();
        AssertThat(player.InvincibleTimer).IsNotNull();
        AssertThat(player.InvincibleTimer.IsStopped()).IsFalse();
    }

    [TestCase]
    public void Test_Number_PowerUp_ApplyEffect()
    {
        int initialBombLimit = player.bombLimit;
        var numberPowerUp = new Number_PowerUp();
        numberPowerUp.ApplyEffect(player);
        
        AssertThat(player.bombLimit).IsEqual(initialBombLimit + 1);
    }
}
