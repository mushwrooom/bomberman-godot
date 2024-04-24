using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;


public partial class HUD : Node2D
{

    private int bombCountP1;
    private int bombCountP2;
    private Label bombCountLabelP1;
    private Label bombCountLabelP2;
    private Label blastCountLabelP1, numberCountLabelP1, obstacleLabel1, ghostLabel1, invLabel1, rollLabel1, detLabel1;
    private Label blastCountLabelP2, numberCountLabelP2, obstacleLabel2, ghostLabel2, invLabel2, rollLabel2, detLabel2;
    private Player player1;
    private Player player2;

   private int lastPlayer1BlastPowerUpCount,lastPlayer1NumberPowerUpCount , lastp1obstacle, last1ghost, last1inv, last1roll, last1det=0;
    private int lastPlayer2BlastPowerUpCount, lastPlayer2NumberPowerUpCount, lastp2obstacle , last2ghost, last2inv, last2roll, last2det= 0; 



   

    public override void _Ready()
    {
        bombCountLabelP1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/bomb_count");
        bombCountLabelP2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/bomb_count2");
        blastCountLabelP1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/BlastShow");
        blastCountLabelP2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/BlastShow");
        numberCountLabelP1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/NumberShow");
        numberCountLabelP2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/NumberShow");
        obstacleLabel1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/ObstacleShow");
        obstacleLabel2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/ObstacleShow");
        ghostLabel1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/GhostShow");
        ghostLabel2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/GhostShow");
        invLabel1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/InvincSHow");
        invLabel2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/InvincSHow");
        rollLabel1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/Roller_skate_show");
        rollLabel2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/Roller_skate_show");
        detLabel1 = GetNode<Label>("MarginContainer/BoxContainer/HBoxContainer/Deetonator_show");
        detLabel2 = GetNode<Label>("MarginContainer2/BoxContainer/HBoxContainer/Deetonator_show");
        

        player1 = GetNodeOrNull<Map>("/root/Main/Map").players[0];
        player2 = GetNodeOrNull<Map>("/root/Main/Map").players[1];
        
        UpdateBombDisplay();

    }

 


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

    private int CountNumberPowerUps(List<Generic_PowerUp> powerUps){

        int cnt = 0;
        foreach(var powerup in powerUps){
            if(powerup is Number_PowerUp){
                cnt++;
            }
        }


        return cnt;
    }  


    private int CountObstacle(List<Generic_PowerUp> powerUps){
        int cnt = 0;
        foreach(var pup in powerUps){
            if(pup is Obstacle_PowerUp){
                cnt++;
            }
        }
        return cnt;
    } 

    private int CountGhost(List<Generic_PowerUp> powerUps){
        int cnt = 0;
        foreach(var pup in powerUps){
            if(pup is Ghost_PowerUp){
                cnt++;
            }
        }
        return cnt;
    }

    private int CountInv(List<Generic_PowerUp> powerUps){
       int cnt = 0;
        foreach(var pup in powerUps){
            if(pup is Invincibility_PowerUp){
                cnt++;
            }
        }
        return cnt;
    }

    private int CountRoll(List<Generic_PowerUp> powerUps){
        int cnt = 0;
        foreach(var pup in powerUps){
            if(pup is Roller_PowerUp){
                cnt++;
            }
        }
        return cnt;
    }

    private int CountDet(List<Generic_PowerUp> powerUps){
        int cnt = 0;
        foreach(var pup in powerUps){
            if(pup is Detonator_PowerUp){
                cnt++;
            }
        }
        return cnt;
    }



    private void UpdateBombDisplay()
    {

        bombCountLabelP1.Text = bombCountP1.ToString();
        bombCountLabelP2.Text = bombCountP2.ToString();
        // blastCountLabelP1.Text = player1BlastPowerUpCount.ToString();
        // blastCountLabelP2.Text = player2BlastPowerUpCount.ToString();

    }



    public override void _Process(double delta)
    {


         base._Process(delta);

        // P1
        int player1BlastPowerUpCount = CountBlastPowerUps(player1.powerUps);
        if (player1BlastPowerUpCount != lastPlayer1BlastPowerUpCount)
        {
            blastCountLabelP1.Text = player1BlastPowerUpCount.ToString();
            lastPlayer1BlastPowerUpCount = player1BlastPowerUpCount;
        }

        int player1NumberPowerUpCount = CountNumberPowerUps(player1.powerUps);
        if(player1NumberPowerUpCount != lastPlayer1NumberPowerUpCount){
            numberCountLabelP1.Text = player1NumberPowerUpCount.ToString();
            lastPlayer1NumberPowerUpCount = player1NumberPowerUpCount;
        }


         int p1ObsCount = CountObstacle(player1.powerUps);
        if (p1ObsCount != lastp1obstacle)
        {
            obstacleLabel1.Text = p1ObsCount.ToString();
            lastp1obstacle = p1ObsCount;
        }

        int p1GhostCnt = CountGhost(player1.powerUps);
        if(p1GhostCnt != last1ghost){

            ghostLabel1.Text = p1GhostCnt.ToString();
            last1ghost = p1GhostCnt;
        }

        int p1InvCnt = CountInv(player1.powerUps);
        if(p1InvCnt != last1inv){

            invLabel1.Text = p1InvCnt.ToString();
            last1inv = p1InvCnt;
        }

        int p1RollCnt = CountRoll(player1.powerUps);

        if(p1RollCnt != last1roll){

            rollLabel1.Text = p1RollCnt.ToString();
            last1roll = p1RollCnt;
        }

        int p1DetCnt = CountDet(player1.powerUps);

        if(p1DetCnt != last1det){

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
        if(player2NumberPowerUpCount != lastPlayer2NumberPowerUpCount){
            numberCountLabelP2.Text = player2NumberPowerUpCount.ToString();
            lastPlayer2NumberPowerUpCount = player2NumberPowerUpCount;
        }

        int p2ObsCount = CountObstacle(player2.powerUps);
        if (p2ObsCount != lastp2obstacle)
        {
            obstacleLabel2.Text = p2ObsCount.ToString();
            lastp2obstacle = p2ObsCount;
        }

        int p2GhostCnt = CountGhost(player2.powerUps);
        if(p1GhostCnt != last2ghost){

            ghostLabel2.Text = p1GhostCnt.ToString();
            last2ghost = p2GhostCnt;
        }


          int p2InvCnt = CountInv(player2.powerUps);
        if(p2InvCnt != last2inv){

            invLabel2.Text = p2InvCnt.ToString();
            last2inv = p2InvCnt;
        }


        int p2RollCnt = CountRoll(player2.powerUps);

        if(p2RollCnt != last2roll){

            rollLabel2.Text = p2RollCnt.ToString();
            last2roll = p2RollCnt;
        }


        int p2DetCnt = CountDet(player2.powerUps);

        if(p2DetCnt != last2det){

            detLabel2.Text = p2DetCnt.ToString();
            last2det = p2DetCnt;
        }

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
