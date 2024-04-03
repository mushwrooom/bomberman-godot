using Godot;
using System;

public partial class UserInterface : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_play_pressed(){
		GetTree().ChangeSceneToFile("res://main.tscn");
	
	}
	public void _on_settings_pressed(){

		GetTree().ChangeSceneToFile("res://UserInterface/mapUI.tscn");
	}	
	public void _on_quit_pressed(){
		GetTree().Quit();
	}

	

}