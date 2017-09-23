using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;
using CatlikeCoding.SimplexNoise;

public class LevelGenerator : MonoSingleton<LevelGenerator>
{

    public Terrain Terrain;

    public LevelModel Model;

    public GameObject WallTopPrefab;
    public GameObject WallRightPrefab;
    public GameObject WallBottomPrefab;
    public GameObject WallLeftPrefab;
    public GameObject CornerTopLeftPrefab;
    public GameObject CornerTopRightPrefab;
    public GameObject CornerBottomLeftPrefab;
    public GameObject CornerBottomRightPrefab;
    public GameObject FloorPrefab;

    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        TerrainData terrainData = Terrain.terrainData;

        int width = (int)terrainData.size.x;
        int length = (int)terrainData.size.z;

        Model = new LevelModel(width, length);

        for (int z = 0; z < length; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                LevelTile tile = Model.Tiles[x, z];

                if (tile.Opened)
                {
                    CreateFloor(tile);
                    continue;
                }

                GenerateWalls(tile);
            }
        }

        Debug.Log("generated level");
    }

    private void GenerateWalls(LevelTile tile)
    {
        List<GameObject> walls = new List<GameObject>();

        if (tile.WallTop)
        {
            if (tile.WallLeft)
            {
                GameObject w = CreateWall(tile, CornerTopLeftPrefab);
                w.name = tile.ToString() + " Corner Top Left";
                walls.Add(w);
            }
            else if (tile.WallRight)
            {
                GameObject w = CreateWall(tile, CornerTopRightPrefab);
                w.name = tile.ToString() + " Corner Top Right";
                walls.Add(w);
            }
            else
            {
                GameObject w = CreateWall(tile, WallTopPrefab);
                w.name = tile.ToString() + " Wall Top";
                walls.Add(w);
            }
        }

        if (tile.WallBottom)
        {
            if (tile.WallLeft)
            {
                GameObject w = CreateWall(tile, CornerBottomLeftPrefab);
                w.name = tile.ToString() + " Corner Bottom Left";
                walls.Add(w);
            }
            else if (tile.WallRight)
            {
                GameObject w = CreateWall(tile, CornerBottomRightPrefab);
                w.name = tile.ToString() + " Corner Bottom Right";
                walls.Add(w);
            }
            else
            {
                GameObject w = CreateWall(tile, WallBottomPrefab);
                w.name = tile.ToString() + " Wall Bottom";
                walls.Add(w);
            }
        }

        if (tile.WallRight && !tile.WallTop && !tile.WallBottom)
        {
            GameObject w = CreateWall(tile, WallRightPrefab);
            w.name = tile.ToString() + " Wall Right";
            walls.Add(w);
        }

        if (tile.WallLeft && !tile.WallTop && !tile.WallBottom)
        {
            GameObject w = CreateWall(tile, WallLeftPrefab);
            w.name = tile.ToString() + " Wall Left";
            walls.Add(w);
        }

        tile.Walls = walls;
    }

    private GameObject CreateWall(LevelTile tile, GameObject prefab)
    {
        GameObject wall = GameObject.Instantiate(prefab);
        wall.transform.position = LevelHelpers.WorldPosFromTilePos(tile.X, tile.Z);

        return wall;
    }

    private GameObject CreateFloor(LevelTile tile)
    {
        GameObject floor = GameObject.Instantiate(FloorPrefab);
        floor.transform.position = LevelHelpers.WorldPosFromTilePos(tile.X, tile.Z);
        floor.name = tile.ToString() + " Floor";

        return floor;
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
