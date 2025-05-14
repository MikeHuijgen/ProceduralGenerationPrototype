using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField, Range(1,100)] private int width;
    [SerializeField, Range(1,100)] private int height;
    [SerializeField] private float perlinNoiseZoomScale;
    [SerializeField] private GenerationRuleSet generationRuleSet;


    private List<Block> _blocks = new List<Block>();

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

        InstantiateTerrainBlocks();

        InstantiateTerrainDecoration();
    }

    private void InstantiateTerrainBlocks()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var xCoord = (float)x / width * perlinNoiseZoomScale + xOffset;
                var zCoord = (float)z / height * perlinNoiseZoomScale + zOffset;

                var sample = Mathf.PerlinNoise(xCoord, zCoord);
                var newYMagnitude = Mathf.RoundToInt(sample * generationRuleSet.yMagnitude);


                var textureColor = new Color(sample,sample,sample);
                perlinNoiseTexture.SetPixel(x, z ,textureColor);

                var correctBlock = GetCorrectBlockByRuleHeight(sample);

                for (int y = newYMagnitude; y > 0; y--)
                {
                    var newPos = new Vector3(x, y, z);

                    if (y == newYMagnitude)
                    {
                        InstantiateBlock(correctBlock,newPos);
                        continue;
                    }
                    else if (correctBlock.GetBlockType == Block.BlockType.Water || correctBlock.GetBlockType == Block.BlockType.DeepWater)
                    {
                        InstantiateBlock(correctBlock,newPos);
                        continue;
                    }
                    else
                    {
                        InstantiateBlock(generationRuleSet.underGroundBlock, newPos);
                    }
                }
            }
        }

        perlinNoiseTexture.filterMode = FilterMode.Point;
        perlinNoiseTexture.wrapMode = TextureWrapMode.Clamp;
        perlinNoiseTexture.Apply();
    }

    private void InstantiateBlock(Block targetBlock, Vector3 position)
    {
        var newBlock = Instantiate(targetBlock, position, Quaternion.identity);
        _blocks.Add(newBlock);
        if (newBlock == null) return;
        newBlock.transform.parent = transform;        
    }

    private Block GetCorrectBlockByRuleHeight(float perlinNoiseSample)
    {
        Block correctBlock = null;
        foreach (var rule in generationRuleSet.terrainRules)
        {
            if (perlinNoiseSample <= rule.ruleHight)
            {
                correctBlock = rule.terrainPrefab;
                break;
            }
        } 

        return correctBlock;
    }

    private void InstantiateTerrainDecoration()
    {
        foreach (var block in _blocks)
        {
            if (block == null || block.GetBlockType != Block.BlockType.Grass) continue;

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
            block.DestroyBlock();
        }

        _blocks.Clear();
    }

    public Texture2D GetTexture => perlinNoiseTexture;
    public int textureWidth => width;
    public int textureHeight => height;
}
