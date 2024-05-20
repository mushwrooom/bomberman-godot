using Godot;
using System;

/// <summary>
/// Represents a monster in the game. Inherits from CharacterBody3D.
/// </summary>
public partial class Monster : CharacterBody3D
{
    public int Speed { get; set; } = 1;

    public Vector3 _targetVelocity = Vector3.Zero;
    private Random _random = new Random();
    private Vector3[] directions = {
        new Vector3(1, 0, 0),  // East
        new Vector3(-1, 0, 0), // West
        new Vector3(0, 0, 1),  // North
        new Vector3(0, 0, -1)  // South
    };


    public override void _Ready()
    {
        prev = Position;

    }

    /// <summary>
    /// Callback method for timer timeouts.
    /// </summary>
    public void _on_timer_timeout()
    {
        // // Generate a random direction
        // float randX = (float)(_random.NextDouble() * 2.0 - 1.0); // Random value between -1 and 1
        // float randZ = (float)(_random.NextDouble() * 2.0 - 1.0); // Random value between -1 and 1
        // Vector3 direction = new Vector3(randX, 0, randZ).Normalized(); // Normalize to ensure consistent speed

        // // Update the target velocity based on the random direction
        // _targetVelocity.X = direction.X * Speed;
        // _targetVelocity.Z = direction.Z * Speed;
        ChangeDirection();
    }

    private Vector3 prev;
    public override void _PhysicsProcess(double delta)
    {
        
        if (Position.DistanceTo(prev) < 0.01 * Speed) 
        ChangeDirection();
        prev = Position;
        Move(delta);
    }

    public void Move(double delta)
    {

        // Set the body's velocity property
        Velocity = _targetVelocity;
        MoveAndSlide();
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision3D collision = GetSlideCollision(i);
            Object body = collision.GetCollider();
            if (body is Blast)
            {
                QueueFree();
            }
        }
    }


    public void ChangeDirection()
    {
        // Randomly select a new direction to move in
        Vector3 newDirection = directions[_random.Next(directions.Length)];

        _targetVelocity = newDirection * Speed;
         Velocity = _targetVelocity; // velocity update
    }

}
