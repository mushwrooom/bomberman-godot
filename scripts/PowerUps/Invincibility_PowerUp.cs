using Godot;
using System;
using System.Threading.Tasks;


public partial class Invincibility_PowerUp : Generic_PowerUp
{
    public int duration = 5;
    private bool appearInvincible = true;

    //Change the mesh of the player to a different color during the duration of the powerup
    public void ShowInvincibility(Player player)
    {
        appearInvincible = true;
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(0.1f, 0.1f, 0.1f, 0.5f)
        };
    }

    //turn back the color of the player when the duration ends 
    public void HideInvincibility(Player player)
    {
        appearInvincible = false;
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)
        };
    }


    // apply the invincibility powerup
    public override void ApplyEffect(Player player)
    {
        // Get rid of the previous invincibility powerup
        if (player.InvincibleTimer != null)
        {
            ShowInvincibility(player);
            player.InvincibleTimer.Stop();
            player.InvincibleTimer.QueueFree();
        }
        // Add the timer to the player and connect the signal
        player.InvincibleTimer = new Timer();
        player.AddChild(player.InvincibleTimer);
        player.InvincibleTimer.Timeout += () => EndEffect(player);

        //call the function to start the powerup
        StartEffect(player);
    }

    private void StartEffect(Player player)
    {
        ShowInvincibility(player);
        player.IsInvincible = true;
        //disable collision for blast so that the player doesnt die
        player.SetCollisionMaskValue(7, false);

        player.InvincibleTimer.OneShot = true;
        player.InvincibleTimer.Start(duration);
    }


    //end the invincibility powerup
    public override async void EndEffect(Player player)
    {
        //remove powerup
        player.powerUps.Remove(this);

        //flashing effect
        int ms = 500;
        for (int i = 0; i < 6; i++)
        {
            if (appearInvincible)
                HideInvincibility(player);
            else
                ShowInvincibility(player);
            await Task.Delay(TimeSpan.FromMilliseconds(ms));

            // If the player has another powerup, terminate the end sequence
            if (HasPowerUp<Invincibility_PowerUp>(player))
            {
                ShowInvincibility(player);
                QueueFree();
                return;
            }
        }

        // Undo everything
        HideInvincibility(player);
        player.SetCollisionMaskValue(7, true);
        player.IsInvincible = false;
        player.InvincibleTimer.QueueFree();
        player.InvincibleTimer = null;
    }
}
