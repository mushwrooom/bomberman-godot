using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Linq;



public partial class HUD : Node
{
    //Initializing the bomb count for 2 players 
    private int bombCountP1;
    private int bombCountP2;

    //Getting the label from the godot to change the hud
    private Label bombCountLabelP1;
    private Label bombCountLabelP2;
    private Label blastCountLabelP1, numberCountLabelP1, obstacleLabel1, ghostLabel1, invLabel1, rollLabel1, detLabel1;
    private Label blastCountLabelP2, numberCountLabelP2, obstacleLabel2, ghostLabel2, invLabel2, rollLabel2, detLabel2;
    public Player player1;
    public Player player2;
    public Timer timer;
    private Label timerLabel;

    // Player last power-up counts for state change detection
    private Dictionary<Type, int> lastCountsP1 = new Dictionary<Type, int>();
    private Dictionary<Type, int> lastCountsP2 = new Dictionary<Type, int>();

    private int lastPlayer1BlastPowerUpCount, lastPlayer1NumberPowerUpCount, lastp1obstacle, last1ghost, last1inv, last1roll, last1det = 0;
    private int lastPlayer2BlastPowerUpCount, lastPlayer2NumberPowerUpCount, lastp2obstacle, last2ghost, last2inv, last2roll, last2det = 0;

    public override void _Ready()
    {
        MarginContainer p1 = GetNode<MarginContainer>("MarginContainer/HBox/P1Container");
        MarginContainer p2 = GetNode<MarginContainer>("MarginContainer/HBox/P2Container");
        bombCountLabelP1 = p1.GetNode<Label>("HBoxContainer/bomb_count");
        bombCountLabelP2 = p2.GetNode<Label>("HBoxContainer/bomb_count2");
        blastCountLabelP1 = p1.GetNode<Label>("HBoxContainer/BlastShow");
        blastCountLabelP2 = p2.GetNode<Label>("HBoxContainer/BlastShow");
        numberCountLabelP1 = p1.GetNode<Label>("HBoxContainer/NumberShow");
        numberCountLabelP2 = p2.GetNode<Label>("HBoxContainer/NumberShow");
        obstacleLabel1 = p1.GetNode<Label>("HBoxContainer/ObstacleShow");
        obstacleLabel2 = p2.GetNode<Label>("HBoxContainer/ObstacleShow");
        ghostLabel1 = p1.GetNode<Label>("HBoxContainer/GhostShow");
        ghostLabel2 = p2.GetNode<Label>("HBoxContainer/GhostShow");
        invLabel1 = p1.GetNode<Label>("HBoxContainer/InvincSHow");
        invLabel2 = p2.GetNode<Label>("HBoxContainer/InvincSHow");
        rollLabel1 = p1.GetNode<Label>("HBoxContainer/Roller_skate_show");
        rollLabel2 = p2.GetNode<Label>("HBoxContainer/Roller_skate_show");
        detLabel1 = p1.GetNode<Label>("HBoxContainer/Deetonator_show");
        detLabel2 = p2.GetNode<Label>("HBoxContainer/Deetonator_show");
        timerLabel = GetNode<Label>("MarginContainer/HBox/Timer/Label");
       
        UpdateBombDisplay();

    }




    private int CountPowerUpsOfType<T>(List<Generic_PowerUp> powerUps) where T : Generic_PowerUp
    {
        return powerUps.OfType<T>().Count();
    }

    private void UpdateLabel<T>(Player player, Label label, Dictionary<Type, int> lastCounts) where T : Generic_PowerUp
    {
        int currentCount = CountPowerUpsOfType<T>(player.powerUps);
        if (!lastCounts.TryGetValue(typeof(T), out int lastCount) || currentCount != lastCount)
        {
            label.Text = currentCount.ToString();
            lastCounts[typeof(T)] = currentCount;
        }
    }

     private void UpdateBombDisplay()
    {
        bombCountLabelP1.Text = $"{bombCountP1}/{lastCountsP1.GetValueOrDefault(typeof(Number_PowerUp), 0) + 1}";
        bombCountLabelP2.Text = $"{bombCountP2}/{lastCountsP2.GetValueOrDefault(typeof(Number_PowerUp), 0) + 1}";
    }



    public override void _Process(double delta)
    {
        
        timerLabel.Text = timer.TimeLeft.ToString("0.0");
        UpdatePlayerHUD(player1, bombCountLabelP1, blastCountLabelP1, numberCountLabelP1, obstacleLabel1, ghostLabel1, invLabel1, rollLabel1, detLabel1, lastCountsP1);
        UpdatePlayerHUD(player2, bombCountLabelP2, blastCountLabelP2, numberCountLabelP2, obstacleLabel2, ghostLabel2, invLabel2, rollLabel2, detLabel2, lastCountsP2);

    }

    private void UpdatePlayerHUD(Player player, Label bombLabel, Label blastLabel, Label numberLabel, Label obstacleLabel, Label ghostLabel, Label invLabel, Label rollLabel, Label detLabel, Dictionary<Type, int> lastCounts)
    {
        UpdateLabel<Blast_PowerUp>(player, blastLabel, lastCounts);
        UpdateLabel<Number_PowerUp>(player, numberLabel, lastCounts);
        UpdateLabel<Obstacle_PowerUp>(player, obstacleLabel, lastCounts);  
        UpdateLabel<Ghost_PowerUp>(player, ghostLabel, lastCounts);
        UpdateLabel<Invincibility_PowerUp>(player, invLabel, lastCounts);
        UpdateLabel<Roller_PowerUp>(player, rollLabel, lastCounts);
        UpdateLabel<Detonator_PowerUp>(player, detLabel, lastCounts);

        int currentBombCount = player.Bombs.Count;
        if (currentBombCount != bombCountP1 && player == player1 || currentBombCount != bombCountP2 && player == player2)
        {
            if (player == player1)
                bombCountP1 = currentBombCount;
            else
                bombCountP2 = currentBombCount;
            UpdateBombDisplay();
        }
    }


}
