using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class Ghost_PowerUp : Generic_PowerUp
{
    public int duration = 5;
    bool appearGhost = false;
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

    public override async void EndEffect(Player player)
    {
        player.powerUps.Remove(this);

        int ms = 500;
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

        // Check if the player is on a forbidden tile
        if (IsOnForbiddenTile(player))
            player.Die();

        // Undo everything
        HideGhostEffect(player);
        player.SetCollisionMaskValue(5, true);
        player.SetCollisionMaskValue(3, true);
        player.GhostTimer.QueueFree();
        player.GhostTimer = null;
    }

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

