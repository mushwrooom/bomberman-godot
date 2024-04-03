using Godot;
using System;

public abstract class Generic_PowerUp
{
	protected int duration;
	protected bool is_cumulative;

	//abstract classes
	public abstract void applyEffect(Player player);
	public virtual void endEffect(Player player)
	{

	}
}