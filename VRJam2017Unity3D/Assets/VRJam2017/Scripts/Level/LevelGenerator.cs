using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;

public class LevelGenerator : MonoSingleton<LevelGenerator>
{
    public Terrain terrain;

    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Debug.Log("generated level");
    }
}
