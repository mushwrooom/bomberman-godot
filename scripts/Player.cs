using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a player character in the game with abilities like movement, placing bombs, and collecting power-ups.
/// </summary>
public partial class Player : CharacterBody3D
{
    private int playerId;
    private List<Bomb> bombs;
    private int bombLimit;
    private int blastRange;
    private Dictionary<ControlScheme, Key> controls;
    private List<Generic_PowerUp> powerUps;

    /// <summary>
    /// Gets the player's ID.
    /// </summary>
    /// <returns>The player ID.</returns>
    public int GetPlayerID() { return playerId; }

    /// <summary>
    /// Gets the bombs currently placed by the player.
    /// </summary>
    /// <returns>An array of Bomb objects.</returns>
    public Bomb[] GetBombs() { return bombs.ToArray(); }

    /// <summary>
    /// Gets the limit of bombs this player can place.
    /// </summary>
    /// <returns>The bomb limit.</returns>
    public int GetBombLimit() { return bombLimit; }

    /// <summary>
    /// Determines the player's current position on a Tile.
    /// </summary>
    /// <remarks>
    /// Utilizes a raycast from the player's position downwards to find the Tile they are standing on.
    /// </remarks>
    /// <returns>The Tile beneath the player.</returns>
    public Tile GetPosition()
    {
        var spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(Vector3.Down, GlobalPosition);
        var result = spaceState.IntersectRay(query);
        return result["collider"].As<Tile>();
    }

    /// <summary>
    /// Sets the player's ID.
    /// </summary>
    /// <param name="id">The new ID for the player.</param>
    public void SetPlayerId(int id)
    {
        playerId = id;
    }

    /// <summary>
    /// Sets the control schemes for the player.
    /// </summary>
    /// <param name="layout">A dictionary mapping from ControlScheme to Key representing the controls.</param>
    public void SetControlSchemes(Dictionary<ControlScheme, Key> layout)
    {
        controls = layout;
    }

    /// <summary>
    /// Initialization method called when the node enters the scene tree.
    /// </summary>
    /// <remarks>
    /// Sets up the player's controls based on their assigned name.
    /// </remarks>
    public override void _Ready()
    {
        // Set controls for checking (will be done by the map)
        if (Name == "Player1")
        {
            controls = new Dictionary<ControlScheme, Key>();
            controls.Add(ControlScheme.MOVE_UP, Key.W);
            controls.Add(ControlScheme.MOVE_DOWN, Key.S);
            controls.Add(ControlScheme.MOVE_LEFT, Key.A);
            controls.Add(ControlScheme.MOVE_RIGHT, Key.D);
        }
        if (Name == "Player2")
        {
            controls = new Dictionary<ControlScheme, Key>();
            controls.Add(ControlScheme.MOVE_UP, Key.Up);
            controls.Add(ControlScheme.MOVE_DOWN, Key.Down);
            controls.Add(ControlScheme.MOVE_LEFT, Key.Left);
            controls.Add(ControlScheme.MOVE_RIGHT, Key.Right);
        }
    }

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    public void Move()
    {
        int speed = 10; // Speed in units per second
        Vector3 targetVelocity = Vector3.Zero; // Desired movement direction and speed

        var direction = Vector3.Zero;
        // Determine direction based on key presses
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_LEFT])) direction.X -= 1.0f;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_RIGHT])) direction.X += 1.0f;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_UP])) direction.Z -= 1.0f;
        if (Input.IsKeyPressed(controls[ControlScheme.MOVE_DOWN])) direction.Z += 1.0f;

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
    }

    /// <summary>
    /// Updates the player every frame.
    /// </summary>
    /// <param name="delta">Time elapsed since the last frame.</param>
    public override void _PhysicsProcess(double delta)
    {
        Move();
    }

    /// <summary>
    /// Places a bomb at the player's current location.
    /// </summary>
    public void PlaceBomb()
    {
        bombLimit--;
    }

    public void PlaceObstacle()
    {
        // Implementation for placing an obstacle
    }

    /// <summary>
    /// Adds a power-up to the player's collection.
    /// </summary>
    /// <param name="powerup">The power-up to add.</param>
    public void AddPowerUp(Generic_PowerUp powerup)
    {
        powerUps.Add(powerup);
    }

    /// <summary>
    /// Removes a power-up from the player's collection.
    /// </summary>
    /// <param name="powerup">The power-up to remove.</param>
    public void RemovePowerUp(Generic_PowerUp powerup)
    {
        powerUps.Remove(powerup);
    }
}
