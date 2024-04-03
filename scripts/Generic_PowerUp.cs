using Godot;
using System;

public abstract class Generic_PowerUp
{
	protected int duration;
	protected bool is_cumulative;

	//abstract classes
	public abstract void applyEffect(player player);
	public virtual void endEffect(player player)
	{

	}
}