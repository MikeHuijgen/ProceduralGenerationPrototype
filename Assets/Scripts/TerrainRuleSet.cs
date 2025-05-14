using System;
using UnityEngine;

[Serializable]
public class TerrainRuleSet
{
    public Block terrainPrefab;
    [Range(0,1)]public float ruleHight;
}
