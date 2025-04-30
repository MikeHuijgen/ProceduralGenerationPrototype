using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject grassPrefab;

    [SerializeField] private float perlinNoiseZoomScale;

    private List<GameObject> _blocks = new List<GameObject>();

    [ExecuteInEditMode]
    public void GenerateTerrain()
    {
        DestroyTerrain();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newPos = new Vector3(x,0,y);
                var newBlock = Instantiate(grassPrefab, newPos, Quaternion.identity);
                _blocks.Add(newBlock);
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
