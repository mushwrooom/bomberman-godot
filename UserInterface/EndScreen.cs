using Godot;
using System;

public partial class EndScreen : Node
{
	private Global global;
	private Label endMessage;
    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");
		endMessage = GetNode<Label>("MarginContainer/CenterContainer/VBoxContainer/Label");
		endMessage.Text = global.endMessage;
    }
    public void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}
	public void _on_button_2_pressed(){
		GetTree().ChangeSceneToFile("res://UserInterface/ui.tscn");
	}
	public void _on_button_3_pressed(){
		GetTree().Quit();
	}
}
