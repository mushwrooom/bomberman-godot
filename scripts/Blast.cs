using Godot;
using System;
using System.Threading.Tasks;

public partial class Blast : RigidBody3D
{
    int fadeTime = 300;
    public override void _Ready()
    {
        DestroyBlast();
    }

    private async void DestroyBlast(){
		await Task.Delay(TimeSpan.FromMilliseconds(fadeTime));
		QueueFree();
	}
}
