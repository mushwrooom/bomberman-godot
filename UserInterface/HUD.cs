using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;


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
        // player1 = GetNode<Map>("/root/Main/Map").GetPlayers()[0];
        // player2 = GetNode<Map>("/root/Main/Map").GetPlayers()[1];

        UpdateBombDisplay();

    }



    //Function for counting the power ups. When player picksup the power up it will increment the power up by one and show it on the hud.
    private int CountBlastPowerUps(List<Generic_PowerUp> powerUps)
    {

        int count = 0;
        foreach (var powerUp in powerUps)
        {
                    if (powerUp is Blast_PowerUp)
            {
                count++;
            }
        }
        return count;
    }

    private int CountNumberPowerUps(List<Generic_PowerUp> powerUps)
    {

        int cnt = 0;
        foreach (var powerup in powerUps)
        {
            if (powerup is Number_PowerUp)
            {
                cnt++;
            }
        }


        return cnt;
    }


    private int CountObstacle(Player player)
    {
        return player.ObstacleStock;
    }

    private int CountGhost(List<Generic_PowerUp> powerUps)
    {
        int cnt = 0;
        foreach (var pup in powerUps)
        {
            if (pup is Ghost_PowerUp)
            {
                cnt++;
            }
        }
        return cnt;
    }

    private int CountInv(List<Generic_PowerUp> powerUps)
    {
        int cnt = 0;
        foreach (var pup in powerUps)
        {
            if (pup is Invincibility_PowerUp)
            {
                cnt++;
            }
        }
        return cnt;
    }

    private int CountRoll(List<Generic_PowerUp> powerUps)
    {
        int cnt = 0;
        foreach (var pup in powerUps)
        {
            if (pup is Roller_PowerUp)
            {
                cnt++;
            }
        }
        return cnt;
    }

    private int CountDet(List<Generic_PowerUp> powerUps)
    {
        int cnt = 0;
        foreach (var pup in powerUps)
        {
            if (pup is Detonator_PowerUp)
            {
                cnt++;
            }
        }
        return cnt;
    }



    private void UpdateBombDisplay()
    {

        bombCountLabelP1.Text = bombCountP1.ToString() + "/" + (lastPlayer1NumberPowerUpCount+1);
        bombCountLabelP2.Text = bombCountP2.ToString() + "/" + (lastPlayer2NumberPowerUpCount+1);
        // blastCountLabelP1.Text = player1BlastPowerUpCount.ToString();
        // blastCountLabelP2.Text = player2BlastPowerUpCount.ToString();

    }



    public override void _Process(double delta)
    {
        timerLabel.Text = timer.TimeLeft.ToString("0.0");

        // P1
        // Updates the display of blast power-ups for player 1 if the count has changed.
        // This function checks the number of blast power ups player 1 currently has and updates the UI label to show this count.
        // It only updates the UI and internal count if there has been a change from the last known count to avoid unnecessary updates.

        // Count the current blast power-ups for player 1
        int player1BlastPowerUpCount = CountBlastPowerUps(player1.powerUps);

        // Check if the current count is different from the last recorded count
        if (player1BlastPowerUpCount != lastPlayer1BlastPowerUpCount)
        {
            blastCountLabelP1.Text = player1BlastPowerUpCount.ToString();
            lastPlayer1BlastPowerUpCount = player1BlastPowerUpCount;
        }

        int player1NumberPowerUpCount = CountNumberPowerUps(player1.powerUps);
        if (player1NumberPowerUpCount != lastPlayer1NumberPowerUpCount)
        {
            numberCountLabelP1.Text = player1NumberPowerUpCount.ToString();
            lastPlayer1NumberPowerUpCount = player1NumberPowerUpCount;
        }


        int p1ObsCount = CountObstacle(player1);
        if (p1ObsCount != lastp1obstacle)
        {
            obstacleLabel1.Text = player1.ObstacleStock.ToString();
            lastp1obstacle = p1ObsCount;
        }

        int p1GhostCnt = CountGhost(player1.powerUps);
        if (p1GhostCnt != last1ghost)
        {

            ghostLabel1.Text = p1GhostCnt.ToString();
            last1ghost = p1GhostCnt;
        }

        int p1InvCnt = CountInv(player1.powerUps);
        if (p1InvCnt != last1inv)
        {

            invLabel1.Text = p1InvCnt.ToString();
            last1inv = p1InvCnt;
        }

        int p1RollCnt = CountRoll(player1.powerUps);

        if (p1RollCnt != last1roll)
        {

            rollLabel1.Text = p1RollCnt.ToString();
            last1roll = p1RollCnt;
        }

        int p1DetCnt = CountDet(player1.powerUps);

        if (p1DetCnt != last1det)
        {

            detLabel1.Text = p1DetCnt.ToString();
            last1det = p1DetCnt;
        }






        // P2
        int player2BlastPowerUpCount = CountBlastPowerUps(player2.powerUps);
        if (player2BlastPowerUpCount != lastPlayer2BlastPowerUpCount)
        {
            blastCountLabelP2.Text = player2BlastPowerUpCount.ToString();
            lastPlayer2BlastPowerUpCount = player2BlastPowerUpCount;
        }

        int player2NumberPowerUpCount = CountNumberPowerUps(player2.powerUps);
        if (player2NumberPowerUpCount != lastPlayer2NumberPowerUpCount)
        {
            numberCountLabelP2.Text = player2NumberPowerUpCount.ToString();
            lastPlayer2NumberPowerUpCount = player2NumberPowerUpCount;
        }

        int p2ObsCount = CountObstacle(player2);
        if (p2ObsCount != lastp2obstacle)
        {
            obstacleLabel2.Text = player2.ObstacleStock.ToString();
            lastp2obstacle = p2ObsCount;
        }

        int p2GhostCnt = CountGhost(player2.powerUps);
        if (p1GhostCnt != last2ghost)
        {

            ghostLabel2.Text = p1GhostCnt.ToString();
            last2ghost = p2GhostCnt;
        }


        int p2InvCnt = CountInv(player2.powerUps);
        if (p2InvCnt != last2inv)
        {

            invLabel2.Text = p2InvCnt.ToString();
            last2inv = p2InvCnt;
        }


        int p2RollCnt = CountRoll(player2.powerUps);

        if (p2RollCnt != last2roll)
        {

            rollLabel2.Text = p2RollCnt.ToString();
            last2roll = p2RollCnt;
        }


        int p2DetCnt = CountDet(player2.powerUps);

        if (p2DetCnt != last2det)
        {

            detLabel2.Text = p2DetCnt.ToString();
            last2det = p2DetCnt;
        }

        // Checks and updates the bomb counts for both players if there have been any changes.

        if (player1 != null)
        {
            int currentBombCount = player1.Bombs.Count;
            if (currentBombCount != bombCountP1)
            {
                bombCountP1 = currentBombCount;
                UpdateBombDisplay();
            }
        }

        if (player2 != null)
        {
            int currentBombCountP2 = player2.Bombs.Count;
            if (currentBombCountP2 != bombCountP2)
            {
                bombCountP2 = currentBombCountP2;
                UpdateBombDisplay();
            }
        }


    }
}
