using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject waterPrefab;

    [SerializeField] private float perlinNoiseZoomScale;

    private List<GameObject> _blocks = new List<GameObject>();

    private float xOffset;
    private float yOffset;

    [ExecuteInEditMode]
    public void GenerateTerrain()
    {
        DestroyTerrain();

        xOffset = Random.Range(0f, 999999f);
        yOffset = Random.Range(0f, 999999f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var xCoord = (float)x / width * perlinNoiseZoomScale + xOffset;
                var yCoord = (float)y / height * perlinNoiseZoomScale + yOffset;

                var sample = Mathf.PerlinNoise(xCoord, yCoord);
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
            }
        }
    }

    public void DestroyTerrain()
    {
        if (_blocks.Count <= 0) return;

        foreach (var Block in _blocks)
        {
            Block.GetComponent<Block>().DestroyBlock();
        }

        _blocks.Clear();
    }
}
