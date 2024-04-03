using System;
using System.Collections.Generic;
using System.Xml.Resolvers;
using Godot;

public partial class Map : Godot.Node {
	private PackedScene _tilex;
	private Tile[,] tiles;
	private List<Player> players = new List<Player>();
	private List<Monster> monsters = new List<Monster>();
	private List<Box> boxes = new List<Box>();
	private Godot.Timer timer;

	private Tile TileX;
	public override void _Ready() {
		_tilex = GD.Load<PackedScene>("res://tile.tscn");
		CreateField(10, 10); 
		//SetupPlayers();
		//SetupMonsters();
	}

	public List<Player> GetPlayers()
	{
		return players;
	}

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

	public void CreateField(int width, int height) {
    float tileSize = 30.0f; // Each tile is 30m x 30m
    tiles = new Tile[width, height];
    for (int x = 0; x < width; x++) {
        for (int z = 0; z < height; z++) {
            Node3D tileInstance = _tilex.Instantiate<Node3D>();
            tileInstance.Name = "Tile_" + x + "_" + z;
            GetNode("Tiles").AddChild(tileInstance);

            // Position tiles based on their index and tile size
            Vector3 worldPosition = new Vector3(x * tileSize, 0, z * tileSize);
            tileInstance.Transform = new Transform3D(Basis.Identity, worldPosition);

            // Optionally scale the tile if needed
            // This assumes the original tile is 1m^2 and needs to be scaled up to 30m^2
            tileInstance.Scale = new Vector3(tileSize, 1, tileSize); // Adjust the y scale as necessary

            // Debug print to verify tile creation
            GD.Print("Created tile at position: ", worldPosition);
        }
    }
}

	public void SetupPlayers(int playerCount, List<Dictionary<Control,Key>> playerControls) {
		// Place the 2 players wherever for now
		var player1 = new Player(); // Player should be a class you define elsewhere
		var player2 = new Player();
		players.Add(player1);
		players.Add(player2);
		// Add players to the scene
	}

	public void SetupMonsters(int monsterCount) {
		// Place around 4 monsters for now
		for (int i = 0; i < 4; i++) {
			var monster = new Monster(); // Monster should be a class you define elsewhere
			monsters.Add(monster);
			// Add monsters to the scene
		}
	}

	//shrink function
	private void Shrink()
	{

	}
}