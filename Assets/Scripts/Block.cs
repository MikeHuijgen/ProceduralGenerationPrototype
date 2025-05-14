using System;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockType{
        Grass,
        Sand,
        Water,
        DeepWater,
        Stone
    }

    [SerializeField] private BlockType blockType;

    public BlockType GetBlockType => blockType; 

    [ExecuteAlways]
    public void DestroyBlock()
    {
        DestroyImmediate(gameObject);
    }
}
