using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ChunkGenerator : MonoBehaviour
{
    [HideInInspector] public TerrainManager manager;

    private Chunk chunk;
    private Vector2 offset = Vector2.zero;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    
    private void Start()
    {
        //Generate(new Vector2Int(0, 0)); // Remove later
        manager = TerrainManager.instance;
    }

    public void Generate(Vector2Int pos, Vector2 offset)
    {
        if(chunk == null) chunk = new Chunk(pos);

        chunk.position = pos;

        this.offset = offset;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateMesh();
    }
    
    private float GenerateNoise(float noiseX, float noiseY, int octaves, float frequency, float rarity, float distortion)
    {
        float result = 0;

        for(int i = 0; i < octaves; i++)
        {
            float nx = noiseX * frequency * i;
            float nz = noiseY * frequency * i;
            result += Mathf.Pow(Mathf.PerlinNoise(nx + Mathf.PerlinNoise(noiseX * i, noiseY * i) * distortion,
                nz + Mathf.PerlinNoise(nx * i, nz * i) * distortion), rarity) / (i + 2);
        }
        return result;
    }

    private Vector3 GenerateVertex(int i, int j, float noiseX, float noiseY)
    {
        Vector2 chunkSize = manager.chunkSize;
        int subdivisions = manager.subdivisions;

        float noise = 0;

        foreach(NoiseFeature noiseFeature in manager.noiseFeatures)
        {
            noise += GenerateNoise(noiseX * chunkSize.x + offset.x, noiseY * chunkSize.x + offset.y, noiseFeature.octaves, noiseFeature.frequency, noiseFeature.rarity, noiseFeature.distortion) 
                * noiseFeature.strength;
        }

        float height = noise * chunkSize.y - chunkSize.y / 2;

        return new Vector3(i * chunkSize.x / subdivisions, height, j * chunkSize.x / subdivisions);
    }

    public void GenerateMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        int subdivisions = manager.subdivisions;

        for (int i = 0; i <= subdivisions; i++)
        {
            for(int j = 0; j <= subdivisions; j++)
            {
                verts.Add(GenerateVertex(i, j, i / (float)subdivisions + chunk.position.x, j / (float)subdivisions + chunk.position.y));
                uvs.Add(new Vector2(i / (float)subdivisions, j / (float)subdivisions));
            }
        }

        for(int j = 0; j < subdivisions; j++)
        {
            for(int i = 0; i < subdivisions; i++)
            {
                int idx = i + j * (subdivisions + 1);

                tris.Add(idx);
                tris.Add(idx + 1);
                tris.Add(idx + subdivisions + 1);

                tris.Add(idx + 1);
                tris.Add(idx + subdivisions + 2);
                tris.Add(idx + subdivisions + 1);
            }
        }

        vertices = verts.ToArray();
        triangles = tris.ToArray();
        this.uvs = uvs.ToArray();

        UpdateMesh();
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        if(manager.generateColliders)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}
