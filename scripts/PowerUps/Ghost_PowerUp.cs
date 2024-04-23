using Godot;
using System;
using System.Threading.Tasks;

public partial class Ghost_PowerUp : Generic_PowerUp
{
    
    public bool IsGhost;

    public void ShowGhostEffect(Player player)
    {
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(1.0f, 1.0f, 1.0f, 0.5f)
        };   
    }

    public void HideGhostEffect(Player player)
    {
        var meshInstance = player.GetNode<MeshInstance3D>("MeshInstance3D");
        meshInstance.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(1.0f, 1.0f, 1.0f, 1.0f) 
        };
    }

    public bool IsOnForbiddenTile(Player player)
    {
        var GetCurrentTile = player.GetPosition().Content;
        bool isForbidden = (GetCurrentTile!= null);
        //GD.Print("Is on Forbidden Tile: " + isForbidden + " | Layer: " + GetCurrentTile);
        return isForbidden;
    }

    public void Die()
    {
        if (IsGhost)
        {
            return;
        }
        QueueFree();
    }
    
    public override void ApplyEffect(Player player)
    {
        player.Set("IsGhost", false);
        ShowGhostEffect(player);
        //disable for bomb
        player.SetCollisionMaskValue(5,false);
        //disable for wall & box
        player.SetCollisionMaskValue(3, false);
        StartEffect(player);
    }


    public override void EndEffect(Player player)
    {
        player.Set("IsGhost", false);
        HideGhostEffect(player);

        player.SetCollisionMaskValue(5, true);
        player.SetCollisionMaskValue(3, true);


        if (IsOnForbiddenTile(player))
        {
            player.Die();
        }
    }

    private async void StartEffect(Player player)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(10000)); // 10 seconds
        EndEffect(player);
    }
    
}

