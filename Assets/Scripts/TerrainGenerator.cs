using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField, Range(1,100)] private int width;
    [SerializeField, Range(1,100)] private int height;
    [SerializeField] private float perlinNoiseZoomScale;
    [SerializeField] private float yMagnitude = 2;  
    [SerializeField] private GenerationRuleSet generationRuleSet;


    [SerializeField]private List<GameObject> _blocks = new List<GameObject>();

    private float xOffset;
    private float zOffset;

    [SerializeField, HideInInspector]private Texture2D perlinNoiseTexture;

    [ExecuteInEditMode]
    public void GenerateTerrain()
    {
        DestroyTerrain();

        perlinNoiseTexture = new Texture2D(width,height);

        xOffset = Random.Range(0f, 999999f);
        zOffset = Random.Range(0f, 999999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var xCoord = (float)x / width * perlinNoiseZoomScale + xOffset;
                var zCoord = (float)z / height * perlinNoiseZoomScale + zOffset;

                var sample = Mathf.PerlinNoise(xCoord, zCoord);
                var newYMagnitude = sample * yMagnitude;



                var textureColor = new Color(sample,sample,sample);
                perlinNoiseTexture.SetPixel(x, z ,textureColor);

                var newPos = new Vector3(x, Mathf.RoundToInt(newYMagnitude),z);
                GameObject newBlock = null;

                foreach (var rule in generationRuleSet.terrainRules)
                {
                    if (sample <= rule.ruleHight)
                    {
                        newBlock = Instantiate(rule.terrainPrefab, newPos, Quaternion.identity);
                        break;
                    }
                }

                perlinNoiseTexture.filterMode = FilterMode.Point;
                perlinNoiseTexture.wrapMode = TextureWrapMode.Clamp;
                perlinNoiseTexture.Apply();

                _blocks.Add(newBlock);
                if (newBlock == null) continue;
                newBlock.transform.parent = transform;
            }
        }

        foreach (var block in _blocks)
        {
            if (block == null) continue;

            if (block.TryGetComponent<Block>(out var component))
            {
                if (component == null || component.GetBlockType != Block.BlockType.Grass) continue;
            }


            var allowDecorationNumber = Random.Range(0f,1f);
            var allowDecoration = allowDecorationNumber < generationRuleSet.decorationAppearanceAmount;

            if(!allowDecoration) continue;

            var randomNumber = Random.Range(0f,1f);
            GameObject newDecoration = null;

            foreach (var decorations in generationRuleSet.decorations)
            {
                if (randomNumber <= decorations.appearanceRate)
                {
                    newDecoration = Instantiate(decorations.decorationPrefab, new Vector3(block.transform.position.x,block.transform.position.y + .5f,block.transform.position.z), Quaternion.identity);
                    newDecoration.transform.parent = block.transform;
                    break;
                }
            }
        }
    }

    public void DestroyTerrain()
    {
        perlinNoiseTexture = null;
        if (_blocks.Count <= 0) return;

        foreach (var block in _blocks)
        {
            if (block == null)
            {
                continue;
            }
            block.GetComponent<Block>().DestroyBlock();
        }

        _blocks.Clear();
    }

    public Texture2D GetTexture => perlinNoiseTexture;
    public int textureWidth => width;
    public int textureHeight => height;
}
