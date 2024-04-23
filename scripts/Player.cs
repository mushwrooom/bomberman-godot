using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a player character in the game with abilities like movement, placing bombs, and collecting power-ups.
/// </summary>
public partial class Player : CharacterBody3D
{
    private int playerId;
    public List<Bomb> Bombs = new();
    private int bombLimit = 2;
    private int blastRange = 2;
    public bool HasDetonator = false;
    private Dictionary<ControlScheme, Key> controls;
    private List<Generic_PowerUp> powerUps = new();
    private PackedScene bombScene;
    private bool alive=true;
    

    //getters
    public int GetPlayerID() 
    { 
        if (Name == "Player1")
            playerId=1;
        else if(Name == "Player2")
            playerId=2;
        return playerId; 
    }

    public Tile GetPosition()
    {
        var ray = GetNode<RayCast3D>("RayCast3D");
        return ray.GetCollider() as Tile;
    }
    public bool isAlive(){return alive;}

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

    public void Die()
    {
        QueueFree();  
    }

    public void SetControlSchemes(Dictionary<ControlScheme, Key> layout)
    {
        controls = layout;
    }

    /// Initialization method called when the node enters the scene tree.
    public override void _Ready()
    {
        //add itself to players in map
        GetNode<Map>("/root/Main/Map").players.Add(this);

        //create bomb scene
        bombScene = GD.Load<PackedScene>("res://scenes/bomb.tscn");

        // Set controls for checking (will be done by the map)
        if (Name == "Player1")
        {
            controls = new Dictionary<ControlScheme, Key>
            {
                { ControlScheme.MOVE_UP, Key.W },
                { ControlScheme.MOVE_DOWN, Key.S },
                { ControlScheme.MOVE_LEFT, Key.A },
                { ControlScheme.MOVE_RIGHT, Key.D },
                { ControlScheme.PLACE_BOMB, Key.Space }
            };

        }
        if (Name == "Player2")
        {
            controls = new Dictionary<ControlScheme, Key>
            {
                { ControlScheme.MOVE_UP, Key.Up },
                { ControlScheme.MOVE_DOWN, Key.Down },
                { ControlScheme.MOVE_LEFT, Key.Left },
                { ControlScheme.MOVE_RIGHT, Key.Right },
                { ControlScheme.PLACE_BOMB, Key.Enter }
            };
        }


    }

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    public int speed = 5;
    public void Move(double delta)
    {
        //speed = 5; // Speed in units per second
        Vector3 targetVelocity = Vector3.Zero; // Desired movement direction and speed

        var direction = Vector3.Zero;
        // Determine direction based on key presses
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_LEFT])) direction.X = -1;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_RIGHT])) direction.X = 1;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_UP])) direction.Z = -1;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_DOWN])) direction.Z = 1;

        // Normalize direction to prevent faster diagonal movement
        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
        }

        // Calculate the target velocity based on direction and speed
        targetVelocity.X = direction.X * speed;
        targetVelocity.Z = direction.Z * speed;

        // Apply the movement
        Velocity = targetVelocity;
        MoveAndSlide(); // Smooths out collisions with walls or obstacles
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision3D collision = GetSlideCollision(i);
            Object body = collision.GetCollider();
            CheckCollisions(body);
        }
        // Check collsiion after moving
    }

    public void CheckCollisions(Object collider)
    {
        if (collider is Powerup powerup)
        {
            powerUps.Add(powerup.GetPowerUpType());
            powerup.GetPowerUpType().ApplyEffect(this);
            powerup.QueueFree();
        }
        else if (collider is Monster || collider is Blast)
        {
            Die();
        }
    }


    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        // Check if the event is an InputEventKey, and if the key was released.
        if (@event is InputEventKey eventKey && eventKey.Pressed == false)
        {
            // Check if the released key is the one mapped to PLACE_BOMB in your dictionary.
            if (eventKey.Keycode == controls[ControlScheme.PLACE_BOMB] && eventKey.IsReleased())
            {
                PlaceBomb();
            }
        }
    }

    private Bomb IsOnBomb = null;
    public override void _PhysicsProcess(double delta)
    {
        Move(delta);


        // Check if the player has exited the bomb tile
        if (IsOnBomb != null && GetPosition().Content != IsOnBomb)
        {
            RemoveCollisionExceptionWith(IsOnBomb);
            IsOnBomb = null;
        }
    }


    /// <summary>
    /// Places a bomb at the player's current location.
    /// </summary>
    public void PlaceBomb()
    {
        // Stop function if bomb is at limit
        if(Bombs.Count >= bombLimit){
            // var tempBombs = Bombs;
            // if(HasDetonator)
            // {
            //     GD.Print(tempBombs);
            //     foreach (Bomb bomb in tempBombs)
            //     {
            //         bomb?.Explode();
            //     }
            // }
            return;
        }
            
        
        // Terminate if there is something inside tile
        if (GetPosition().Content != null) return;


        // Create an instance of the bomb
        Bomb bombInstance = bombScene.Instantiate<Bomb>();

        // No collision with the bomb until further notice
        AddCollisionExceptionWith(bombInstance);
        IsOnBomb = bombInstance;

        // Set the bomb's position to the player's position
        bombInstance.Position = GetPosition().Position + Vector3.Up * .5f;
        GetPosition().SetContent(bombInstance);

        bombInstance.BlastRange = blastRange;
        bombInstance.HasDetonator = HasDetonator;
        bombInstance.player = this;

        // Add the bomb instance to the sce
        GetParent().AddChild(bombInstance);
        Bombs.Add(bombInstance);

        bombInstance.ID = Bombs.Count;
        // for(int i = 0; i < Bombs.Count; i++)
        // {
        //     GD.Print(i + " " + Bombs[i].ID);
        // }
    }

    public void PlaceObstacle()
    {

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
