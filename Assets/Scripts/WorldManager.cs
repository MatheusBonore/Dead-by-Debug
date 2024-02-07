using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager:MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] public int seed;
	[SerializeField] public bool useRandomSeed;
	[Space]

	public List<World> worlds = new List<World>();
	public int activeWorld = 0;
	[Space]

	[Header("References")]
	public Transform[] layerParent;
	public Tilemap[] tilemaps;

	// Start is called before the first frame update
	void Start()
	{
		if (useRandomSeed) {
			seed = Random.Range(-1000000, 1000000);
		}

		worlds = new List<World>();
		worlds.Add(new World());
		worlds[0].Initialize(this, 0);
	}

	// Update is called once per frame
	void Update()
	{

	}
}

public class World
{
	public GenerationType type;
	public CustomWorld reference;
	public int seed;

	public Vector2[] octaveOffsets = new Vector2[6];

	public WorldManager world;
	public Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

	public void Initialize(WorldManager _world, int index, int activeWorld = -1, Vector2Int position = new Vector2Int())
	{
		world = _world;
		if (type != GenerationType.Custom) {
			System.Random seedGen = new System.Random(world.seed * (index + 1));
			seed = seedGen.Next(0, 1000000);
			System.Random octGen = new System.Random(seed);

			for (int i = 0; i < octaveOffsets.Length; i++) {
				float offsetX = octGen.Next(-1000000, 1000000);
				float offsetY = octGen.Next(-1000000, 1000000);
				octaveOffsets[i] = new Vector2(offsetX, offsetY);
			}
		} else {
			if (reference == null) {
				Debug.LogError("No Reference World Found!");
			}

			//foreach (ChunkEntry chunk in reference.chunks) { }
		}
	}

	public void CreateChunk(Vector2Int position, bool[,] walls = null)
	{
		Chunk chunk = new Chunk();
		chunk.Initialize();

		chunks.Add(position, chunk);
		PopulateChunk(position);
	}

	public void PopulateChunk(Vector2Int chunkPosition)
	{
		if (type != GenerationType.Custom) {
			for (int x = 0; x < 8; x++) {
				for (int z = 0; z < 8; z++) {
					Vector2Int exactPosition = new Vector2Int(chunkPosition.x * 8 + x, chunkPosition.y * 8 + z);
					//float randomValue = Noise.GetRandomValue(exactPosition, seed);
				}
			}
		}
	}

	public enum GenerationType
	{
		Overworld,
		Custom
	}
}

public class CustomWorld
{
	public List<Vector2Int> chunks;
}

public class Chunk
{
	public void Initialize()
	{

	}
}