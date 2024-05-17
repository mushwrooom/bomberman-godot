using Godot;
using System;

public partial class UserInterface : Node
{
	public void _on_play_pressed(){
		GetTree().ChangeSceneToFile("res://UserInterface/mapUI.tscn");
	}
		
	public void _on_quit_pressed(){
		GetTree().Quit();
	}
}
