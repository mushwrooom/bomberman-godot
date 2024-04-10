using Godot;
using System;
using System.Reflection.PortableExecutable;

public partial class Bomb : Node3D
{
	
	private int blastRange;

	public void setBlastRange(int a)
	{
		blastRange=a;
	}

	public void explode()
	{
		Visible = false;
		GD.Print("explode");
	}

	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void onTimerTimeout()
	{
		explode();
	}
}

