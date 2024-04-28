using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class Ghost_PowerUp : Generic_PowerUp
{
    public int duration = 5;
    bool appearGhost = false;

    //change the appearance of the player to transparent once Ghost powerup is started
    public void ShowGhostEffect(Player player)
    {
        appearGhost = true;
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(0.5f, 1.0f, 1.0f, 0.7f),
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha
        };
    }

    //change the appearance of the player back to its normal shade and revert the transparency
    public void HideGhostEffect(Player player)
    {
        appearGhost = false;
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(1.0f, 1.0f, 1.0f, 1.0f),
            Transparency = BaseMaterial3D.TransparencyEnum.Disabled
        };
    }

    // function to check if the player is on a wall or a box by the end of the powerup
    public bool IsOnForbiddenTile(Player player)
    {
        var GetCurrentTile = player.GetCurrentTile().Content;
        bool isForbidden = GetCurrentTile is Wall || GetCurrentTile is Box;
        return isForbidden;
    }


    public override void ApplyEffect(Player player)
    {
        // Get rid of the previous ghost powerup
        if (player.GhostTimer != null)
        {
            player.GhostTimer.Stop();
            player.GhostTimer.QueueFree();
        }
        // Add the timer to the player and connect the signal
        player.GhostTimer = new Timer();
        player.AddChild(player.GhostTimer);
        player.GhostTimer.Timeout += () => EndEffect(player);

        // Start Effects
        StartEffect(player);
    }

    //Ending Ghost Powerup
    public override async void EndEffect(Player player)
    {
        //remove powerup
        player.powerUps.Remove(this);

        //flashing effect
        int ms = 300; 
        for (int i = 0; i < 6; i++)
        {
            if (appearGhost)
                HideGhostEffect(player);
            else
                ShowGhostEffect(player);
                
            await Task.Delay(TimeSpan.FromMilliseconds(ms));

            // If the player has another ghost powerup, terminate the end sequence
            if (HasPowerUp<Ghost_PowerUp>(player))
            {
                ShowGhostEffect(player);
                QueueFree();
                return;
            }
        }

        // Check if the player is on a forbidden tile, and if the player is, it dies
        if (IsOnForbiddenTile(player))
            player.Die();

        // Undo everything
        HideGhostEffect(player);
        player.SetCollisionMaskValue(5, true);
        player.SetCollisionMaskValue(3, true);
        player.GhostTimer.QueueFree();
        player.GhostTimer = null;
    }

    //Starting Ghost Powerup
    private void StartEffect(Player player)
    {
        ShowGhostEffect(player);
        //disable for bomb
        player.SetCollisionMaskValue(5, false);
        //disable for wall & box
        player.SetCollisionMaskValue(3, false);

        player.GhostTimer.OneShot = true;
        player.GhostTimer.Start(duration);
    }
}

