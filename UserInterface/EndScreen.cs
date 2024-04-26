using Godot;
using System;

public partial class EndScreen : Control
{
	public void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}
}
