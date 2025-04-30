using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField, Range(1,100)] private int width;
    [SerializeField, Range(1,100)] private int height;
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject waterPrefab;

    [SerializeField] private float perlinNoiseZoomScale;

    private List<GameObject> _blocks = new List<GameObject>();

    private float xOffset;
    private float yOffset;

    [SerializeField, HideInInspector]private Texture2D perlinNoiseTexture;

    [ExecuteInEditMode]
    public void GenerateTerrain()
    {
        DestroyTerrain();

        perlinNoiseTexture = new Texture2D(width,height);

        xOffset = Random.Range(0f, 999999f);
        yOffset = Random.Range(0f, 999999f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var xCoord = (float)x / width * perlinNoiseZoomScale + xOffset;
                var yCoord = (float)y / height * perlinNoiseZoomScale + yOffset;

                var sample = Mathf.PerlinNoise(xCoord, yCoord);

                var textureColor = new Color(sample,sample,sample);
                perlinNoiseTexture.SetPixel(x, y ,textureColor);

                var newPos = new Vector3(x,0,y);
                GameObject newBlock = null;

                if (sample > .4f)
                {
                    newBlock = Instantiate(grassPrefab, newPos, Quaternion.identity);
                }
                else
                {
                    newBlock = Instantiate(waterPrefab, newPos, Quaternion.identity);                   
                }

                _blocks.Add(newBlock);
                newBlock.transform.parent = transform;
                perlinNoiseTexture.filterMode = FilterMode.Point;
                perlinNoiseTexture.wrapMode = TextureWrapMode.Clamp;
                perlinNoiseTexture.Apply();
            }
        }
    }

    public void DestroyTerrain()
    {
        if (_blocks.Count <= 0) return;

        perlinNoiseTexture = null;

        foreach (var Block in _blocks)
        {
            Block.GetComponent<Block>().DestroyBlock();
        }

        _blocks.Clear();
    }

    public Texture2D GetTexture => perlinNoiseTexture;
    public int textureWidth => width;
    public int textureHeight => height;
}
