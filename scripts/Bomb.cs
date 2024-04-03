using Godot;
using System;

public partial class Bomb : Node3D
{
	
	private int blastRange;

	public void setBlastRange(int a)
	{
		blastRange=a;
	}

	public void explode()
	{
		
	}

	//func to start ticking (called after placing bomb)

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
