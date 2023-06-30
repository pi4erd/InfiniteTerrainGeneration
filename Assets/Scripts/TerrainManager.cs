using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;

    public int viewDistance = 2;

    public Vector2 chunkSize; // Width / Depth, not Width / Length
    public int subdivisions = 1;

    public NoiseFeature[] noiseFeatures = new NoiseFeature[0];

    public int worldWidth = 1;

    public WorldGenerator worldGenerator;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one instance of TerrainManager is present in the scene! " +
                "Consider deleting one of them to avoid overwritten data.");
        } // We do not return on purpose to ensure that noone makes two instances :imp:
        instance = this;
    }
}
