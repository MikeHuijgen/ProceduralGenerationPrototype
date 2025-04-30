using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    private TerrainGenerator _terrainGenerator;

    void OnEnable()
    {
        _terrainGenerator = (TerrainGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate terrain", GUILayout.Height(30)))
        {
            _terrainGenerator.GenerateTerrain();
        }

        if(GUILayout.Button("Destroy terrain", GUILayout.Height(30)))
        {
            _terrainGenerator.DestroyTerrain();
        }
    }
}
