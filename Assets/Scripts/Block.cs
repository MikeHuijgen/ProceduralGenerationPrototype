using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    [ExecuteAlways]
    public void DestroyBlock()
    {
        DestroyImmediate(gameObject);
    }
}
