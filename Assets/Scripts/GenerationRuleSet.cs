using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenerationRuleSet", menuName = "Scriptable Objects/GenerationRuleSet")]
public class GenerationRuleSet : ScriptableObject
{
    public List<TerrainRuleSet> terrainRules = new List<TerrainRuleSet>();
    public List<DecorationRuleSet> decorations = new List<DecorationRuleSet>();
    [Range(0,1), Tooltip("The higher the number the more change you have to spawn in decorations")] public float decorationAppearanceAmount;
}
