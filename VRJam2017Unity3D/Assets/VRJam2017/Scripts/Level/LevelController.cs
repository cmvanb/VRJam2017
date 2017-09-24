using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;
using CatlikeCoding.SimplexNoise;

public class LevelController : MonoSingleton<LevelController>
{
    public Terrain Terrain;

    public LevelModel Model;

    public Transform WallParent;

    public GameObject WallTopPrefab;
    public GameObject WallRightPrefab;
    public GameObject WallBottomPrefab;
    public GameObject WallLeftPrefab;
    public GameObject CornerTopLeftPrefab;
    public GameObject CornerTopRightPrefab;
    public GameObject CornerBottomLeftPrefab;
    public GameObject CornerBottomRightPrefab;

    public GameObject HeroSpawn;

    public GameObject[] GoblinVillages;

    public GameObject[] HellStartingBuildings;

    [Range(0,1)]
    public float GoblinVillageSpawnChance = 0.1f;

    public void Start()
    {

        Generate();

        Vector3 spawnPosition = LevelHelpers.WorldPosFromTilePos((int)Model.HellSpawn.x, (int)Model.HellSpawn.y);

        CreateObjectOnTile(Model.HeavenSpawnTile, HeroSpawn);

        float buildingSpawnDist = 4;
        foreach(GameObject prefab in HellStartingBuildings)
        {
            float angle = Random.value * Mathf.PI * 2;

            float x = buildingSpawnDist * Mathf.Cos(angle);
            float z = buildingSpawnDist * Mathf.Sin(angle);

            Vector2 pos = Model.HellSpawn + new Vector2(x,z);

            CreateObjectOnTile(Model.Tiles[(int)pos.x, (int)pos.y], prefab);
        }

        foreach(Vector3 roomPosition in Model.Rooms)
        {
            if (Random.value > 1 - GoblinVillageSpawnChance)
            {
                GameObject prefab = GoblinVillages[(int)(Random.value*GoblinVillages.Length)];

                CreateObjectOnTile(Model.Tiles[(int)roomPosition.x, (int)roomPosition.y], prefab);
            }
        }
         
        
        GameManager.Instance.SpawnPlayer(spawnPosition);
    }

    public void Generate()
    {
        TerrainData terrainData = Terrain.terrainData;

        int width = (int)(terrainData.size.x / LevelHelpers.TileSize);
        int length = (int)(terrainData.size.z / LevelHelpers.TileSize);

        Model = new LevelModel(width, length);

        for (int z = 0; z < length; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                LevelTile tile = Model.Tiles[x, z];

                if (tile.Opened)
                {
                    continue;
                }

                UpdateWallsForTile(tile);
            }
        }

        Terrain.transform.parent = transform;

        Debug.Log("generated level");
    }

    public bool IsTileInHell(int x, int z)
    {
        return Model.HellContiguousTiles[x,z] != null;
    }

    public bool IsTileInHeaven(int x, int z)
    {
        return Model.HeavenContiguousTiles[x,z] != null;
    }

    public void Dig(int x, int z)
    {
        if (!LevelHelpers.TileIsInBounds(Model, x, z))
        {
            return;
        }

        LevelTile tile = Model.Tiles[x, z];

        if (tile.Opened)
        {
            return;
        }

        tile.Opened = true;

        List<LevelTile> surroundingTiles = LevelHelpers.GetSurroundingTiles(Model, x, z);

        foreach (LevelTile t in surroundingTiles)
        {
            Model.EvaluateTile(t.X, t.Z);
            UpdateWallsForTile(t);
        }

        Model.UpdateContiguousTilesFrom(x, z);
    }

    private void UpdateWallsForTile(LevelTile tile)
    {
        foreach (GameObject wall in tile.Walls)
        {
            GameObject.Destroy(wall);
        }

        tile.Walls.Clear();

        CreateWalls(tile);
    }

    private void CreateWalls(LevelTile tile)
    {
        if (tile.WallTop)
        {
            // if (tile.WallLeft)
            // {
            //     GameObject w = CreateObjectOnTile(tile, CornerTopLeftPrefab);
            //     w.name = tile.ToString() + " Corner Top Left";
            //     tile.Walls.Add(w);
            // }
            // else if (tile.WallRight)
            // {
            //     GameObject w = CreateObjectOnTile(tile, CornerTopRightPrefab);
            //     w.name = tile.ToString() + " Corner Top Right";
            //     tile.Walls.Add(w);
            // }
            // else
            // {
                GameObject w = CreateObjectOnTile(tile, WallTopPrefab, WallParent);
                w.name = tile.ToString() + " Wall Top";
                tile.Walls.Add(w);
            // }
        }

        if (tile.WallBottom)
        {
            // if (tile.WallLeft)
            // {
            //     GameObject w = CreateObjectOnTile(tile, CornerBottomLeftPrefab);
            //     w.name = tile.ToString() + " Corner Bottom Left";
            //     tile.Walls.Add(w);
            // }
            // else if (tile.WallRight)
            // {
            //     GameObject w = CreateObjectOnTile(tile, CornerBottomRightPrefab);
            //     w.name = tile.ToString() + " Corner Bottom Right";
            //     tile.Walls.Add(w);
            // }
            // else
            // {
                GameObject w = CreateObjectOnTile(tile, WallBottomPrefab, WallParent);
                w.name = tile.ToString() + " Wall Bottom";
                tile.Walls.Add(w);
            // }
        }

        if (tile.WallRight)// && !tile.WallTop && !tile.WallBottom)
        {
            GameObject w = CreateObjectOnTile(tile, WallRightPrefab, WallParent);
            w.name = tile.ToString() + " Wall Right";
            tile.Walls.Add(w);
        }

        if (tile.WallLeft)// && !tile.WallTop && !tile.WallBottom)
        {
            GameObject w = CreateObjectOnTile(tile, WallLeftPrefab, WallParent);
            w.name = tile.ToString() + " Wall Left";
            tile.Walls.Add(w);
        }
    }

    private GameObject CreateObjectOnTile(LevelTile tile, GameObject prefab, Transform parent = null)
    {
        GameObject created = GameObject.Instantiate(prefab);
        created.transform.position = LevelHelpers.WorldPosFromTilePos(tile.X, tile.Z);
        created.transform.parent = (parent == null) ? transform : parent;

        return created;
    }
}
