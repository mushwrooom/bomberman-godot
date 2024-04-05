using System;
using System.Collections.Generic;
using System.Xml.Resolvers;
using Godot;

/// <summary>
/// Represents a game map consisting of tiles, players, monsters, and potentially boxes. It handles the creation and management of these entities.
/// </summary>
public partial class Map : Godot.Node {
	/// <summary>The tile scene template</summary>
	private PackedScene _tilex;
	private Tile[,] tiles;
	private List<Player> players = new List<Player>();
	private List<Monster> monsters = new List<Monster>();
	private List<Box> boxes = new List<Box>();
	private Godot.Timer timer;

	private Tile TileX;

	/// <summary>
    /// Called when the node enters the scene tree. Initializes the map by loading the tile scene and creating the field.
    /// </summary>
	public override void _Ready() {
		_tilex = GD.Load<PackedScene>("res://scenes/tile.tscn");
		CreateField(10, 10); 
		//SetupPlayers();
		//SetupMonsters();
	}

	public List<Player> GetPlayers()
	{
		return players;
	}

	/// <summary>
    /// Placeholder for handling timer timeout events.
    /// </summary>
	private void _on_timer_timeout()
    {
		
    }

	
	/*
	 public void CreateField(int width, int height) {
		
		tiles = new Tile[width, height];
		for (int x = 0; x < width; x++) {
			for (int z = 0; z < height; z++) {
				// instantiate 
				Node3D u = _tilex.Instantiate<Node3D>();
				u.Name = "Tile_" + x + "_" + z;
            	GetNode("Tiles").AddChild(u);
                // Calculate the 3D world position for each tile
            	Vector3 worldPosition = new Vector3(x, 0, z); // Adjust y coordinate as needed
            	u.Transform = new Transform3D(Basis.Identity, worldPosition);
				// Inside the loop, after setting the tile's Transform:
				GD.Print("Created tile at position: ", worldPosition);

				//tiles[x, z] = new Tile(TileType.EMPTY);
				//GD.Print("kdojfofnoap");
				
				//u.Name = "muug";
				//GetNode("Tiles").AddChild(u);
				// Here you'd calculate the 3D world position for each tile
				// Assuming your 3D tiles are spaced 1 unit apart, for example
				//Vector3 worldPosition = new Vector3(x, 0, z);
				//tiles[x, z].WorldPosition = worldPosition;

				// You don't AddChild because Tile is not a Node in this context.
				// Instead, Tile is a logical representation of the map grid.
			}
		}
	}*/

	/// <summary>
    /// Creates a field of tiles with specified width and height. Positions tiles based on their indices and a predefined tile size.
    /// </summary>
    /// <param name="width">The width of the field in tiles.</param>
    /// <param name="height">The height of the field in tiles.</param>
	public void CreateField(int width, int height) {
    float tileSize = 1.0f; 
    tiles = new Tile[width, height];
    for (int x = 0; x < width; x++) {
        for (int z = 0; z < height; z++) {
			// Instantiates a new tile instance from the PackedScene _tilex.
			// _tilex is expected to be a preloaded scene/template of a tile, which is an Area3D node.
            Area3D tileInstance = _tilex.Instantiate<Area3D>();
            tileInstance.Name = "Tile_" + x + "_" + z;
			// Adds the newly created tile instance as a child of the "Tiles" node.
            GetNode("Tiles").AddChild(tileInstance);

            // Position tiles based on their index and tile size
            Vector3 worldPosition = new Vector3(x * tileSize - width/2 + tileInstance.Scale.X/2, 0.5f, z * tileSize - height/2 + tileInstance.Scale.Z/2);
            tileInstance.Position = worldPosition;
        }
    }
}

	 /// <summary>
	/// Sets up player entities in the game world based on the specified player count and control configurations.
	/// </summary>
	public void SetupPlayers(int playerCount, List<Dictionary<Control,Key>> playerControls) {
		var player1 = new Player(); 
		var player2 = new Player();

		players.Add(player1);
		players.Add(player2);
	}

	/// <summary>
    /// Sets up monster entities on the map. Currently adds a fixed number of monsters.
    /// </summary>
    /// <param name="monsterCount">The number of monsters to add (currently unused).</param>
	public void SetupMonsters(int monsterCount) {
		for (int i = 0; i < 4; i++) {
			var monster = new Monster(); 
			monsters.Add(monster);
		}
	}

	/// <summary>
    /// Placeholder for a method to shrink the map or perform a related function.
    /// </summary>
	private void Shrink()
	{

	}
}