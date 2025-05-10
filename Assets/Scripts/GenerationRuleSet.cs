using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenerationRuleSet", menuName = "Scriptable Objects/GenerationRuleSet")]
public class GenerationRuleSet : ScriptableObject
{
    public List<TerrainRuleSet> terrainRules = new List<TerrainRuleSet>();
}
