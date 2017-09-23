using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;
using CatlikeCoding.SimplexNoise;

public class LevelGenerator : MonoSingleton<LevelGenerator>
{
    public void Start()
    {
        Generate();
    }

    public void Generate()
    {

        // DOESN'T WORK NICELY, THANKS UNITY
        //GenerateTerrain();

        Debug.Log("generated level");
    }

/*     public void GenerateTerrain()
    {
        GameObject terrainObj = new GameObject("Terrain");

        // INIT TERRAIN DATA
        TerrainData terrainData = new TerrainData();

        terrainData.size = new Vector3(sizeMeters, heightMeters, sizeMeters);
        terrainData.heightmapResolution = 512;
        terrainData.baseMapResolution = 1024;
        terrainData.SetDetailResolution(1024, 16);

        int heightMapWidth = terrainData.heightmapWidth;
        int heightMapHeight = terrainData.heightmapHeight;

        // GENERATE HEIGHT
        float[,] heightData = FillDataWithNoise();

        terrainData.SetHeights(0, 0, heightData);

        // SETUP TERRAIN OBJ
        TerrainCollider terrainCollider = terrainObj.AddComponent<TerrainCollider>();
        Terrain terrain = terrainObj.AddComponent<Terrain>();

        terrainCollider.terrainData = terrainData;
        terrain.terrainData = terrainData;

        // TEXTURE
        SplatPrototype[] terrainTexture = new SplatPrototype[1];
        terrainTexture[0] = new SplatPrototype();
        terrainTexture[0].texture = (Texture2D)Resources.Load("Textures/Texture_Environment_09");
        terrainData.splatPrototypes = terrainTexture;

        Debug.Log("generated terrain");
    }

    private float[,] FillDataWithNoise(
        int resolution = 512, 
        float frequency = 1f, 
        int octaves = 1, 
        float lacunarity = 2f, 
        float persistence = 0.5f,
        int dimensions = 3,
        NoiseMethodType type = NoiseMethodType.Perlin,
        Gradient coloring = null)
    {
        float[,] resultData = new float[resolution, resolution];

        Vector3 point00 = new Vector3(-0.5f, -0.5f);
        Vector3 point10 = new Vector3(0.5f, -0.5f);
        Vector3 point01 = new Vector3(-0.5f, 0.5f);
        Vector3 point11 = new Vector3(0.5f, 0.5f);

        NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
        float stepSize = 1f / resolution;

        for (int y = 0; y < resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);

            for (int x = 0; x < resolution; x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence).value;
                sample = sample * 0.5f + 0.5f;
                resultData[x, y] = sample;
            }
        }

        return resultData;
    } */
}
