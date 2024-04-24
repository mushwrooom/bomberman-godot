using Godot;
using System;
using System.Linq;

/// <summary>
/// Provides a base class for different types of power-ups in the game. 
/// It defines common properties and methods that all power-ups should have, 
/// such as duration, whether the effect is cumulative, and the logic for applying and ending the effect.
/// </summary>
public abstract partial class Generic_PowerUp : Node
{
    /// <summary>
    /// Applies the effect of the power-up to the specified player.
    /// </summary>
    /// <param name="player">The player to which the power-up effect is applied.</param>
    /// <remarks>
    /// Implementations of this method should specify exactly how the power-up affects the player, 
    /// such as increasing blast range and making the player ghost for a time period. 
    /// </remarks>
    public abstract void ApplyEffect(Player player);

    /// <summary>
    /// Ends the effect of the power-up on the specified player. This method can be overridden by subclasses.
    /// </summary>
    /// <param name="player">The player on which the power-up effect will be ended.</param>
    /// <remarks>
    /// The default implementation does nothing, but subclasses can override this method to specify 
    /// how to reverse the power-up's effects. For example, if the power-up increases the player's speed, 
    /// this method could decrease the speed back to normal.
    /// </remarks>
    public virtual void EndEffect(Player player)
    {
        // Default implementation: No operation.
    }
    protected static bool HasPowerUp<T>(Player player) where T : Generic_PowerUp
    {
        return player.powerUps.Any(p => p is T);
    }
    public override string ToString()
    {
        string s = GetType().Name;
        return s.Remove(s.IndexOf('_'));
    }
}



