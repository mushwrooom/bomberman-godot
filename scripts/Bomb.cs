using Godot;
using System;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

public partial class Bomb : StaticBody3D
{
	// Time values in ms
	public Player player;
	private int chainInterval = 150;
	private int fadeTime = 500;
	private int blastRange;
	private PackedScene blast;

	public void SetBlastRange(int a)
	{
		blastRange = a;
	}

	public void Explode()
	{
		CreateBlast(Vector3.Right);
		CreateBlast(Vector3.Back);
		CreateBlast(Vector3.Left);
		CreateBlast(Vector3.Forward);


		// Destroying the object without losing the reference
		Visible = false;
		GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
		GetTile(Position).SetContent(null);
		player.Bombs.Remove(this);
	}

	private async void CreateBlast(Vector3 direction)
	{
		for (int i = 1; i <= blastRange; i++)
		{
			Tile projectedTile = GetTile(GlobalPosition + direction.Normalized() * i);
			// Cease the function if it goes out of bound or encounters wall
			if (projectedTile == null)
			{
				return;
			}

			// if (projectedTile.Content != null) GD.Print(projectedTile.Content.GetClass());

			if (projectedTile.Content == null)
			{
				RigidBody3D blastInstance = blast.Instantiate<RigidBody3D>();
				blastInstance.LookAtFromPosition(new Vector3(Position.X, 0, Position.Z), new Vector3(projectedTile.GlobalPosition.X, 0, projectedTile.GlobalPosition.Z));
				blastInstance.Position = projectedTile.Position + Vector3.Up * 0.5f;
				GetParent().AddChild(blastInstance);
				DestroyBlast(blastInstance);
			}
			else if (projectedTile.Content is Wall)
			{
				return;
			}
			else if (projectedTile.Content is Box)
			{
				projectedTile.Content.GetChild(0).GetOwner<Box>().Destroy();
				projectedTile.SetContent(null);
				return;
			}
			else if (projectedTile.Content is Bomb)
			{
				(projectedTile.Content as Bomb).Explode();
				projectedTile.SetContent(null);
				return;
			}

			await Task.Delay(TimeSpan.FromMilliseconds(chainInterval));
		}
	}

	private async void DestroyBlast(RigidBody3D blast){
		await Task.Delay(TimeSpan.FromMilliseconds(fadeTime));
		blast.QueueFree();
	}

	public Tile GetTile(Vector3 pos)
	{
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(pos, Vector3.Down * 100, 2);
		var result = spaceState.IntersectRay(query);
		// Null propagation
		return result.Count == 0 ? null : result["collider"].As<Tile>();
	}


	public override void _Ready()
	{
		blast = GD.Load<PackedScene>("res://scenes/blast.tscn");
	}

	public void onTimerTimeout()
	{
		Explode();
	}
}

