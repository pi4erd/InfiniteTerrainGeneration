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
            CleanChildren(tg.worldGenerator.transform);
        }

        if (GUILayout.Button("Generate"))
        {
            CleanChildren(tg.worldGenerator.transform);
            tg.worldGenerator.GenerateWorld();
        }
    }
    private static void CleanChildren(Transform transform)
    {
        var temp = new GameObject[transform.childCount];

        for(int i = 0; i < temp.Length; i++)
        {
            temp[i] = transform.GetChild(i).gameObject;
        }
        foreach(var child in temp)
        {
            DestroyImmediate(child);
        }
    }
}
