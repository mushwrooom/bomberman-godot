using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Resolvers;
using Godot;

/// <summary>
/// Represents a game map consisting of tiles, players, monsters, and potentially boxes. It handles the creation and management of these entities.
/// </summary>
public partial class Map : Node3D
{
	public double shrinkTime = 30.0; 
	private int shrunkCount = 0;
	private Tile[,] tiles;
	private List<Wall> walls = new();
	public List<Player> players = new();
	private List<Monster> monsters = new();
	private List<Box> boxes = new List<Box>();

	private PackedScene _tilex;
	private PackedScene wallScene;
	private PackedScene blastScene;
	public Timer timer;

	/// <summary>
	/// Called when the node enters the scene tree. Initializes the map by loading the tile scene and creating the field.
	/// </summary>
	public override void _Ready()
	{
		// Get reference to all the scenes
		_tilex = GD.Load<PackedScene>("res://scenes/tile.tscn");
		wallScene = GD.Load<PackedScene>("res://scenes/wall.tscn");
		blastScene = GD.Load<PackedScene>("res://scenes/blast.tscn");

		// Get reference to the bordering walls
		Node[] test = GetNode<Node3D>("Walls").GetChildren()[..4].ToArray();
		foreach (Node i in test) walls.Add(i as Wall);

		timer = GetNode<Timer>("Timer");
		timer.WaitTime = shrinkTime;
		timer.Start();
	}

	public List<Player> GetPlayers()
	{
		players = GetNode<Node3D>("Characters/Players").GetChildren().Select(x => x as Player).ToList();
		return players;
	}

	/// <summary>
	/// Placeholder for handling timer timeout events.
	/// </summary>
	private void _on_timer_timeout()
	{
		if (shrunkCount < 5)
		{

			shrunkCount++;
			Shrink();

			// Decrease the time for the next shrink
			double deductTime = shrinkTime / 5;
			timer.WaitTime = shrinkTime - deductTime;
			timer.Start();
		}
	}


	/// <summary>
	/// Creates a field of tiles with specified width and height. Positions tiles based on their indices and a predefined tile size.
	/// </summary>
	/// <param name="width">The width of the field in tiles.</param>
	/// <param name="height">The height of the field in tiles.</param>
	public void CreateField(int width, int height)
	{
		float tileSize = 1.0f;
		tiles = new Tile[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				// Instantiates a new tile instance from the PackedScene _tilex.
				// _tilex is expected to be a preloaded scene/template of a tile, which is an Area3D node.
				Tile tileInstance = _tilex.Instantiate<Tile>();
				tileInstance.Name = "Tile_" + x + "_" + z;
				// Adds the newly created tile instance as a child of the "Tiles" node.
				GetNode("Tiles").AddChild(tileInstance);

				// Position tiles based on their index and tile size
				Vector3 worldPosition = new(x * tileSize - width / 2 + tileInstance.Scale.X / 2, 0.5f, z * tileSize - height / 2 + tileInstance.Scale.Z / 2);
				tileInstance.GlobalPosition = worldPosition;
				tileInstance.FindContent();
			}
		}
	}

	/// <summary>
	/// Shrink the map.
	/// </summary>
	private async void Shrink()
	{
		List<Wall> all = new();
		foreach (Wall wall in walls)
		{
			Wall wallInstance = wallScene.Instantiate<Wall>();
			GetNode<Node3D>("Walls").AddChild(wallInstance);
			wallInstance.Rotation = wall.Rotation;
			wallInstance.Position = wall.Position + wall.Transform.Basis.Z * shrunkCount;
			wallInstance.GetChild<CollisionShape3D>(0).Disabled = true;
			all.Add(wallInstance);
			for (int i = -5; i <= 5; i++)
			{
				Blast blastInstance = blastScene.Instantiate<Blast>();
				AddChild(blastInstance);
				Vector3 newPos = wallInstance.Position + wall.Transform.Basis.X * i;
				blastInstance.Position = newPos;
				blastInstance.SetCollisionMaskValue(5, false);
			}
		}
		await Task.Delay(TimeSpan.FromMilliseconds(100));
		foreach (Wall i in all) i.GetChild<CollisionShape3D>(0).Disabled = false;
	}
}