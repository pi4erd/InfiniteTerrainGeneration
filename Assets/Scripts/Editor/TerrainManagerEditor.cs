using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var tg = (TerrainManager)target;

        if(GUILayout.Button("Clear chunks"))
        {
            WorldGenerator.CleanChildren(tg.worldGenerator.transform);
        }

        if (GUILayout.Button("Generate"))
        {
            WorldGenerator.CleanChildren(tg.worldGenerator.transform);
            tg.worldGenerator.GenerateWorld();
        }
    }
}
