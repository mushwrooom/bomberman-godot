using Godot;
using System;
using System.Linq;

public partial class EndScreen : Node
{
	private Global global;
	private Label endMessage;
	private Button button;
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		endMessage = GetNode<Label>("MarginContainer/CenterContainer/VBoxContainer/Label");
		button = GetNode<Button>("MarginContainer/CenterContainer/VBoxContainer/Button");
		endMessage.Text = global.endMessage;
		if (global.scores.Any(score => score >= global.roundsToWin))
		{
			button.Text = "New Game";
		}
	}
	public void _on_button_pressed()
	{

		if (global.scores.Any(score => score >= global.roundsToWin))
		{
			{
				GetTree().ChangeSceneToFile("res://UserInterface/mapUI.tscn");
			}
		}
		else
		{
			GetTree().ChangeSceneToFile("res://scenes/main.tscn");
		}
	}

	public void _on_button_2_pressed()
	{
		GetTree().ChangeSceneToFile("res://UserInterface/ui.tscn");
	}
	public void _on_button_3_pressed()
	{
		GetTree().Quit();
	}
}
