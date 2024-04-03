using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{
	private int playerId;
	private List<Bomb> bombs;
	private int bombLimit;
	private int blastRange;
	private Dictionary<ControlScheme, Key> controls;
	private List<Generic_PowerUp> powerUps;
	
	//getters
	public int getPlayerID(){return playerId;}
	public  Bomb[] getBombs(){return bombs.ToArray();}
	public int getBombLimit(){return bombLimit;}
	public Tile getPosition() 
	{ 
		//raycast
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(Vector3.Down, GlobalPosition);
		var result = spaceState.IntersectRay(query); 
		return result["collider"].As<Tile>();

		//use rid? exlcude everything else but tile and return the tile that the player is on 
		// var query = PhysicsRayQueryParameters3D.Create(Vector3.Down, Position);
        // query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        // var result = spaceState.IntersectRay(query);

	}
	public void setPlayerId(int a)
	{
		playerId=a;
	}
	public void setControlSchemes(Dictionary<ControlScheme, Key> layout)
	{
		controls=layout;
	}

	 public override void _Ready()
	{
	//	debug -> GD.Print(this.Name);	

	//set controls for checking (will be done by the map)
		if(Name=="Player1"){
			controls=new Dictionary<ControlScheme, Key>();
			controls.Add(ControlScheme.MOVE_UP, Key.W);
			controls.Add(ControlScheme.MOVE_DOWN, Key.S);
			controls.Add(ControlScheme.MOVE_LEFT, Key.A);
			controls.Add(ControlScheme.MOVE_RIGHT, Key.D);	
		
		}
		if(Name=="Player2"){
			controls=new Dictionary<ControlScheme, Key>();
			controls.Add(ControlScheme.MOVE_UP, Key.Up);
			controls.Add(ControlScheme.MOVE_DOWN, Key.Down);
			controls.Add(ControlScheme.MOVE_LEFT, Key.Left);
			controls.Add(ControlScheme.MOVE_RIGHT, Key.Right);	
		}
	}

	public void move(){
		int Speed = 10; //km per second
	    Vector3 _targetVelocity = Vector3.Zero; // 3D vector combining a speed with a direction

		var direction = Vector3.Zero;
		if( Input.IsKeyPressed(controls[ControlScheme.MOVE_LEFT])) direction.X -= 1.0f;
		if( Input.IsKeyPressed(controls[ControlScheme.MOVE_RIGHT])) direction.X += 1.0f;
		if( Input.IsKeyPressed(controls[ControlScheme.MOVE_UP])) direction.Z-= 1.0f;
		if( Input.IsKeyPressed(controls[ControlScheme.MOVE_DOWN])) direction.Z += 1.0f;

		if (direction != Vector3.Zero) //if player presses 2 keys together, it shouldn't move diagonally faster. we normalize it
		{
			direction = direction.Normalized();
		}

	 	// Ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// Moving the character
		Velocity = _targetVelocity;
		MoveAndSlide(); //If it hits a wall midway through a motion, the engine will try to smooth it out 
	}
	public override void _PhysicsProcess(double delta) 	// update the node every frame. 'delta' is the elapsed time since the previous frame.
	{
		move();
	}

	
	//placeBomb
	public void placeBomb()
	{
		bombLimit--;
		
	}

	public void placeObstacle()
	{

	}

	public void addPowerUp(Generic_PowerUp powerup)
	{
		powerUps.Add(powerup);
	}

	public void removePowerUp(Generic_PowerUp powerup)
	{
		powerUps.Remove(powerup);
	}

}
