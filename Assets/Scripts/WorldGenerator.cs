using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] int seed = 0;

    [SerializeField] Transform player;
    [SerializeField] TerrainManager manager;
    [SerializeField] GameObject chunkPrefab;

    [SerializeField] TMP_Text chunksLoadedText;

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
            StartCoroutine(UpdateChunks());
        }
    }
    public IEnumerator UpdateChunks()
    {
        for (int i = -manager.viewDistance; i < manager.viewDistance; i++)
        {
            for (int j = -manager.viewDistance; j < manager.viewDistance; j++)
            {
                Vector2Int playerPos = new Vector2Int((int)(player.position.x / manager.chunkSize.x), (int)(player.position.z / manager.chunkSize.x));
                prevPlayerPos = playerPos;
                Chunk _chunk = new Chunk(new Vector2Int(i, j) + playerPos);

                if (loadedChunks.ContainsKey(_chunk.position)) 
                {
                    loadedChunks[_chunk.position].SetActive(true);
                    continue;
                }

                var chunk = Instantiate(chunkPrefab,
                    new Vector3(_chunk.position.x * manager.chunkSize.x, 0, _chunk.position.y * manager.chunkSize.x),
                    Quaternion.identity);

                chunk.name = $"{_chunk.position}";

                loadedChunks.Add(_chunk.position, chunk);

                chunk.transform.parent = transform;

                chunk.GetComponent<ChunkGenerator>().manager = manager;
                chunk.GetComponent<ChunkGenerator>().Generate(_chunk.position, new Vector2(883, 1924));
                yield return null;
                chunksLoadedText.text = $"Chunks loaded: {transform.childCount}";
            }
        }
        Dictionary<Vector2Int, GameObject> chunksLoadedClone = loadedChunks.ToDictionary(e => e.Key, e => e.Value);
        foreach (KeyValuePair<Vector2Int, GameObject> loadedChunk in chunksLoadedClone)
        {
            Vector2Int temp = loadedChunk.Key - new Vector2Int((int)(player.position.x / manager.chunkSize.x), (int)(player.position.z / manager.chunkSize.x));
            if (Mathf.Max(Mathf.Abs(temp.x), Mathf.Abs(temp.y)) > manager.viewDistance)
            {
                loadedChunk.Value.SetActive(false);
            }
        }
    }
    public IEnumerator UnloadOld()
    {
        yield return null;
    }
    public void RegenerateWorld()
    {
        CleanChildren(transform);
        loadedChunks.Clear();
        StartCoroutine(UpdateChunks());
    }
    public void GenerateWorld()
    {
        seed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(seed);
        RegenerateWorld();
    }

    public static void CleanChildren(Transform transform)
    {
        var temp = new GameObject[transform.childCount];

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = transform.GetChild(i).gameObject;
        }
        foreach (var child in temp)
        {
            DestroyImmediate(child);
        }
    }
}
