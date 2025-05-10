using System;
using UnityEngine;
[Serializable]
public class DecorationRuleSet
{
    public GameObject decorationPrefab;
    [Range(0,1)]public float appearanceRate;
}
