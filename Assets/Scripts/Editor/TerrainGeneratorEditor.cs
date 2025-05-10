using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    private TerrainGenerator _terrainGenerator;
    private Texture2D perlinNoiseTexture;
    private int maxTextureScale = 10;

    void OnEnable()
    {
        _terrainGenerator = (TerrainGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(5);

        if(GUILayout.Button("Generate terrain", GUILayout.Height(30)))
        {
            _terrainGenerator.GenerateTerrain();
        }

        if(GUILayout.Button("Destroy terrain", GUILayout.Height(30)))
        {
            _terrainGenerator.DestroyTerrain();
        }

        perlinNoiseTexture = _terrainGenerator.GetTexture;

        if (perlinNoiseTexture == null)
        {
            GUILayout.Label("There is no texture available");
        }
        else
        {
            var maxInspectorWidth = EditorGUIUtility.currentViewWidth - EditorGUIUtility.currentViewWidth / 2;
            var scale = Mathf.Min(maxTextureScale, Mathf.FloorToInt(maxInspectorWidth / _terrainGenerator.textureWidth));
            var displayHeight = _terrainGenerator.textureHeight * scale;
            var displayWidth = _terrainGenerator.textureWidth * scale;


            GUILayout.Label("Preview perlin texture:");

            Rect rect = GUILayoutUtility.GetRect(displayWidth, displayHeight, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(rect, perlinNoiseTexture, null, ScaleMode.StretchToFill);
        }

        base.OnInspectorGUI();
    }
}
