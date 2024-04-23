using Godot;
using System;
using System.Threading.Tasks;

public partial class Invincibility_PowerUp : Generic_PowerUp
{

    public bool IsInvincible;
    public void ShowInvincibility(Player player)
    {
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(1.0f, 1.0f, 1.0f, 0.5f)
        };
    }

    public void HideInvincibility(Player player)
    {
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)
        };
    }


    public void Die()
    {
        GD.Print("Die function called."); 
        if (IsInvincible)
        {
            GD.Print("Player is invincible, will not die.");
            return;
        }
        GD.Print("Player is not invincible, dying now.");
        QueueFree(); 
    }


    public override void ApplyEffect(Player player)
    {
        IsInvincible = true;
        GD.Print("Invincibility applied");
        ShowInvincibility(player);
        player.SetCollisionMaskValue(4, false); 
        player.SetCollisionMaskValue(7, false);
        StartEffect(player);
    }

    private async void StartEffect(Player player)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(10000)); // 10 seconds
        EndEffect(player);
    }


    public override void EndEffect(Player player)
    {   
        IsInvincible = false;
        HideInvincibility(player);
        player.SetCollisionMaskValue(4, true); 
        player.SetCollisionMaskValue(7, true);
        Die();
    }
}
