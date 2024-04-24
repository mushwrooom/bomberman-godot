using Godot;
using System;
using System.Threading.Tasks;

public partial class Bomb : StaticBody3D
{
	// Time values in ms
	public Player player;
	private int chainInterval = 150;
	private int fadeTime = 500;
	public int BlastRange = 1;
	public bool HasDetonator = false;
	private PackedScene blast;
	bool exploded = false;

	public void Explode()
	{
		// Destroying the object without losing the reference
		exploded = true;
		Visible = false;
		GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
		GetTile(Position).SetContent(null);
		player.Bombs.Remove(this);
		// Create blast in place and then create in 4 directions
		InstantiateBlast(Position, Position + Vector3.Forward);
		CreateBlast(Vector3.Right);
		CreateBlast(Vector3.Back);
		CreateBlast(Vector3.Left);
		CreateBlast(Vector3.Forward);
	}

	private void InstantiateBlast(Vector3 pos, Vector3 dir)
	{
		RigidBody3D blastInstance = blast.Instantiate<RigidBody3D>();
		blastInstance.LookAtFromPosition(Position, dir);
		blastInstance.Position = new Vector3(pos.X, 1.0f, pos.Z);
		GetParent().AddChild(blastInstance);
	}

	private async void CreateBlast(Vector3 direction)
	{
		for (int i = 1; i <= BlastRange; i++)
		{
			Tile projectedTile = GetTile(GlobalPosition + direction.Normalized() * i);
			// Cease the function if it goes out of bound or encounters wall
			if (projectedTile == null)
			{
				return;
			}

			if (projectedTile.Content == null)
			{
				InstantiateBlast(projectedTile.Position,
								new Vector3(projectedTile.Position.X, Position.Y, projectedTile.Position.Z));
			}
			else if (projectedTile.Content is Wall)
			{
				return;
			}
			else if (projectedTile.Content is Box box)
			{
				box.GetChild(0).GetOwner<Box>().Destroy();
				projectedTile.SetContent(null);
				return;
			}
			else if (projectedTile.Content is Bomb bomb)
			{
				bomb.Explode();
				projectedTile.SetContent(null);
				return;
			}

			await Task.Delay(TimeSpan.FromMilliseconds(chainInterval));
		}
	}



	public Tile GetTile(Vector3 pos)
	{
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(pos, Vector3.Down * 100, 2);
		var result = spaceState.IntersectRay(query);

		// If the raycast does not hit anything, return null
		return result.Count == 0 ? null : result["collider"].As<Tile>();
	}


	public override void _Ready()
	{
		blast = GD.Load<PackedScene>("res://scenes/blast.tscn");
	}

	public void onTimerTimeout()
	{
		if (!exploded && !HasDetonator)
			Explode();
	}
}

