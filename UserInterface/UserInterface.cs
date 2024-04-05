using Godot;
using System;

public partial class UserInterface : Control
{
	
	private Main main;
	public override void _Ready(){
		main = GetNode<Main>("/root/Main");
	}
	public void _on_play_pressed(){
		main.StartGame();	
	}
	public void _on_settings_pressed(){

		GetTree().ChangeSceneToFile("res://UserInterface/mapUI.tscn");
	}	
	public void _on_quit_pressed(){
		GetTree().Quit();
	}

	

}
