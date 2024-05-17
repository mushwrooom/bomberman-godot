using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Represents a player character in the game with abilities like movement, placing bombs, and collecting power-ups.
/// </summary>
public partial class Player : CharacterBody3D
{
    [Export]
    private PackedScene bombScene;
    [Export]
    private PackedScene boxScene;
    private int playerId;
    public List<Bomb> Bombs = new();

    public int speed = 5;
    private int bombLimit = 1;
    private int blastRange = 2;
    public bool HasDetonator = false;
    public int ObstacleStock = 0;
    public Godot.Timer GhostTimer = null;
    public Godot.Timer InvincibleTimer = null;
    public bool IsInvincible = false;
    public List<Generic_PowerUp> powerUps = new();
    private Dictionary<ControlScheme, Key> controls;
    private StaticBody3D InsideObject = null;

    //getters
    public int GetPlayerID()
    {
        if (Name == "Player1")
            playerId = 1;
        else if (Name == "Player2")
            playerId = 2;
        return playerId;
    }

    public Tile GetCurrentTile()
    {
        var ray = GetNode<RayCast3D>("RayCast3D");
        return ray.GetCollider() as Tile;
    }

    //setters
    public void SetPlayerId(int id)
    {
        playerId = id;
    }
    public void AddBlastRange()
    {
        blastRange++;
    }
    public void AddBombLimit()
    {
        bombLimit++;
    }

    public void SetControlSchemes(Dictionary<ControlScheme, Key> layout)
    {
        controls = layout;
    }

    /// <summary>
    /// Prepares the player by loading bomb and box scenes if the scenes were null
    /// </summary>
    public override void _Ready()
    {
        bombScene ??= GD.Load<PackedScene>("res://scenes/bomb.tscn");
        boxScene ??= GD.Load<PackedScene>("res://scenes/box.tscn");
    }

    /// <summary>
    /// Handles player input events for placing bombs and obstacles.
    /// </summary>
    /// <param name="@event">The input event to process.</param>
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey eventKey && eventKey.Pressed == false)
        {
            bool hasObstaclePowerUp = powerUps.Any(powerUp => powerUp is Obstacle_PowerUp);
            // Check if the released key is the one mapped to PLACE_BOMB in your dictionary.
            if (eventKey.Keycode == controls[ControlScheme.PLACE_BOMB])
                PlaceBomb();
            // Only place obstacle if the player has the power-up
            else if (hasObstaclePowerUp && eventKey.Keycode == controls[ControlScheme.PLACE_OBSTACLE]) 
                PlaceObstacle();
        }
    }
        
    /// <summary>
    /// Updates the player's movement and checks for exiting the current tile.
    /// </summary>
    /// <param name="delta">The time elapsed since the last physics update.</param>
    public override void _PhysicsProcess(double delta)
    {
        Move(delta);

        // Check if the player has exited the occupied tile
        if (InsideObject != null && InsideObject.IsInsideTree() && GetCurrentTile().Content != InsideObject)
        {
            RemoveCollisionExceptionWith(InsideObject);
            InsideObject = null;
        }
    }

    /// <summary>
    /// Handles the player's death, removing them from the scene if not invincible.
    /// </summary>
    public void Die()
    {
        if(!IsInvincible)
        {
            GD.Print("Player " + GetPlayerID() + " has died");
            QueueFree();
        }
    }

    /// <summary>
    /// Calculates and applies the player's movement based on input.
    /// </summary>
    /// <param name="delta">The time since the last update.</param>
    public void Move(double delta)
    {
        Vector3 targetVelocity = Vector3.Zero; 
        var direction = Vector3.Zero;

        // Determine direction based on key presses
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_LEFT])) direction.X = -1;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_RIGHT])) direction.X = 1;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_UP])) direction.Z = -1;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_DOWN])) direction.Z = 1;

        // Normalize direction to prevent faster diagonal movement
        if (direction != Vector3.Zero)
            direction = direction.Normalized();

        // Calculate the target velocity based on direction and speed
        targetVelocity.X = direction.X * speed;
        targetVelocity.Z = direction.Z * speed;

        // Apply the movement
        Velocity = targetVelocity;
        MoveAndSlide(); // Smooths out collisions with walls or obstacles
        
        // Check collsiion after moving
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision3D collision = GetSlideCollision(i);
            Object body = collision.GetCollider();
            CheckCollisions(body);
        } 
    }

    /// <summary>
    /// Checks and handles collisions with power-ups, monsters, or blasts. 
    /// </summary>
    /// <param name="collider">The object collided with.</param>
    public void CheckCollisions(Object collider)
    {
        if (collider is Powerup powerup)
        {
            powerUps.Add(powerup.GetPowerUpType());
            powerup.GetPowerUpType().ApplyEffect(this);
            GD.Print("Player " + (playerId + 1) + " has collected a " + powerup.GetPowerUpType());
            powerup.Free();
        }
        else if (collider is Monster || collider is Blast)
            Die();
    }

    /// <summary>
    /// Places a bomb at the player's current position if the player hasn't reached the bomb limit
    /// </summary>
    public void PlaceBomb()
    {
        // Stop the placement of bombs if the player has reached the limit
        if (Bombs.Count >= bombLimit)
        {
            var tempBombs = new List<Bomb>(Bombs); // Create a copy of the Bombs list since the original will be modified during the loop
            if (HasDetonator)
                tempBombs.ForEach(bomb => bomb.Explode());
            return;
        }

        // Terminate if there is something inside tile
        if (GetCurrentTile().Content != null) return;

        // Create an instance of the bomb
        Bomb bombInstance = bombScene.Instantiate<Bomb>();

        // Set the bomb's position to the player's position and add it to the scene
        bombInstance.Position = GetCurrentTile().Position + Vector3.Up * .5f;
        GetCurrentTile().SetContent(bombInstance);
        GetNode<Node>("/root/Main/Misc").AddChild(bombInstance);

        // Set the bomb's properties
        bombInstance.BlastRange = blastRange;
        bombInstance.HasDetonator = HasDetonator;
        bombInstance.player = this;

        // Add the bomb instance to the scene
        Bombs.Add(bombInstance);

        // No collision with the bomb until the player exits the bomb tile
        AddCollisionExceptionWith(bombInstance);
        InsideObject = bombInstance;
    }

    /// <summary>
    /// Places an obstacle at the player's current position if available.
    /// </summary>
    public void PlaceObstacle()
    {
        if (ObstacleStock <= 0) return;
        // Terminate if there is something inside tile
        if (GetCurrentTile().Content != null) return;

        Box boxInstance = boxScene.Instantiate<Box>();

        // Set the box's position to the player's position and add it to the scene
        boxInstance.Position = GetCurrentTile().Position + Vector3.Up * .5f;
        GetCurrentTile().SetContent(boxInstance);
        GetNode<Node>("/root/Main/Map").AddChild(boxInstance);

        // Set the box's properties
        boxInstance.PlacedObstacle = true;

        // Deduct from stock
        ObstacleStock--;

        // No collision with the box until the player exits the box tile
        AddCollisionExceptionWith(boxInstance);
        InsideObject = boxInstance;

    }

    public void AddPowerUp(Generic_PowerUp powerup)
    {
        powerUps.Add(powerup);
    }

    public void RemovePowerUp(Generic_PowerUp powerup)
    {
        powerUps.Remove(powerup);
    }
}
