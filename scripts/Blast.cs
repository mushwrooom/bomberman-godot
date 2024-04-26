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

    // Method to asynchronously destroy the blast after a certain delay
    private async void DestroyBlast(){
        // Delay the execution of the following code by the specified fadeTime
		await Task.Delay(TimeSpan.FromMilliseconds(fadeTime));

        //free the node
		QueueFree();
	}
}
