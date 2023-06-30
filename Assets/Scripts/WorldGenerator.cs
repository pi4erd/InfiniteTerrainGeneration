using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] TerrainManager manager;
    [SerializeField] GameObject chunkPrefab;

    Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();

    Vector2Int prevPlayerPos;

    private void Start()
    {
        manager = TerrainManager.instance;
        prevPlayerPos = new Vector2Int((int)(player.position.x / manager.chunkSize.x), (int)(player.position.z / manager.chunkSize.x));

        GenerateWorld();
    }
    private void Update()
    {
        Vector2Int playerPos = new Vector2Int((int)(player.position.x / manager.chunkSize.x), (int)(player.position.z / manager.chunkSize.x));
        if(prevPlayerPos != playerPos)
        {
            UpdateChunks();
        }
    }
    public void UpdateChunks()
    {
        for (int i = -manager.viewDistance; i < manager.viewDistance; i++)
        {
            for (int j = -manager.viewDistance; j < manager.viewDistance; j++)
            {
                Vector2Int playerPos = new Vector2Int((int)(player.position.x / manager.chunkSize.x), (int)(player.position.z / manager.chunkSize.x));
                prevPlayerPos = playerPos;
                Chunk _chunk = new Chunk(new Vector2Int(i, j) + playerPos);

                if (loadedChunks.ContainsKey(_chunk.position)) continue;

                var chunk = Instantiate(chunkPrefab,
                    new Vector3(_chunk.position.x * manager.chunkSize.x, 0, _chunk.position.y * manager.chunkSize.x),
                    Quaternion.identity);

                chunk.name = $"{i},{j}";

                loadedChunks.Add(new Vector2Int(i, j) + playerPos, chunk);

                chunk.transform.parent = transform;

                chunk.GetComponent<ChunkGenerator>().manager = manager;
                chunk.GetComponent<ChunkGenerator>().Generate(_chunk.position);
            }
        }
    }
    public void GenerateWorld()
    {
        loadedChunks.Clear();
        UpdateChunks();
    }
}
