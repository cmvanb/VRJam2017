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
    public GameObject DigMarkerPrefab;

    public GameObject HeroSpawn;

    public GameObject[] GoblinVillages;

    public GameObject[] HellStartingBuildings;

    public Material CeilingMaskMaterial;

    [Range(0,1)]
    public float GoblinVillageSpawnChance = 0.1f;

    private Texture2D ceilingMaskTexture = null;

    public void Start()
    {
        Generate();

        GameManager.Instance.SpawnPlayer();
    }

    public void Generate()
    {
        // Build level model.
        TerrainData terrainData = Terrain.terrainData;

        int width = LevelHelpers.TileCountX;
        int length = LevelHelpers.TileCountZ;

        Model = new LevelModel(width, length);

        // Update the objects for each tile.
        for (int z = 0; z < length; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                LevelTile tile = Model.Tiles[x, z];

                if (tile.Opened)
                {
                    continue;
                }

                UpdateObjectsForTile(tile);
            }
        }

        // Create the heaven base.
        CreateObjectOnTile(Model.HeavenSpawnTile, HeroSpawn);

        // Create the hell base.
        float buildingSpawnDist = 4;

        foreach(GameObject prefab in HellStartingBuildings)
        {
            float angle = Random.value * Mathf.PI * 2;

            float x = buildingSpawnDist * Mathf.Cos(angle);
            float z = buildingSpawnDist * Mathf.Sin(angle);

            Vector2 pos = Model.HellSpawn + new Vector2(x,z);

            CreateObjectOnTile(Model.Tiles[(int)pos.x, (int)pos.y], prefab);
        }

        // Create the goblin villages.
        foreach(Vector3 roomPosition in Model.Rooms)
        {
            if (Random.value > 1 - GoblinVillageSpawnChance)
            {
                GameObject prefab = GoblinVillages[(int)(Random.value*GoblinVillages.Length)];

                CreateObjectOnTile(Model.Tiles[(int)roomPosition.x, (int)roomPosition.y], prefab);
            }
        }

        // Create the ceiling mask.
        CreateCeilingMask();
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
        UpdateTileDigMarker(tile);

        List<LevelTile> surroundingTiles = LevelHelpers.GetSurroundingTiles(Model, x, z);

        foreach (LevelTile t in surroundingTiles)
        {
            Model.EvaluateTile(t.X, t.Z);
            UpdateObjectsForTile(t);
        }

        Model.UpdateContiguousTilesFrom(x, z);
        UpdateCeilingMask();
    }

    public void UpdateTileDigMarker(LevelTile tile)
    {
        // If there is a marker already...
        if (tile.DigMarker != null)
        {
            // and the tile isn't marked for digging, or it's opened now - then remove it
            if (!tile.MarkedForDigging || tile.Opened)
            {
                GameObject.Destroy(tile.DigMarker);
            }
        }
        // If there is no marker yet...
        else
        {
            // and the tile is marked for digging - the add it
            if (tile.MarkedForDigging)
            {
                tile.DigMarker = GameObject.Instantiate(DigMarkerPrefab);
                tile.DigMarker.transform.position = LevelHelpers.WorldPosFromTilePosSetY(
                    tile.X, 
                    tile.Z, 
                    LevelHelpers.TileDigMarkerPositionY);
                tile.DigMarker.transform.parent = transform;
            }
        }
    }

    private void UpdateObjectsForTile(LevelTile tile)
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
            GameObject w = CreateObjectOnTile(tile, WallTopPrefab, WallParent, true);
            w.name = tile.ToString() + " Wall Top";
            tile.Walls.Add(w);
        }

        if (tile.WallBottom)
        {
            GameObject w = CreateObjectOnTile(tile, WallBottomPrefab, WallParent, true);
            w.name = tile.ToString() + " Wall Bottom";
            tile.Walls.Add(w);
        }

        if (tile.WallRight)
        {
            GameObject w = CreateObjectOnTile(tile, WallRightPrefab, WallParent, true);
            w.name = tile.ToString() + " Wall Right";
            tile.Walls.Add(w);
        }

        if (tile.WallLeft)
        {
            GameObject w = CreateObjectOnTile(tile, WallLeftPrefab, WallParent, true);
            w.name = tile.ToString() + " Wall Left";
            tile.Walls.Add(w);
        }
    }

    private GameObject CreateObjectOnTile(LevelTile tile, GameObject prefab, Transform parent = null, bool isWall = false)
    {
        GameObject created = GameObject.Instantiate(prefab);

        Vector3 worldPosition = isWall ? 
            LevelHelpers.WorldPosFromTilePosSetY(tile.X, tile.Z, LevelHelpers.WallPositionY) 
            : LevelHelpers.WorldPosFromTilePos(tile.X, tile.Z);

        created.transform.position = worldPosition;
        created.transform.parent = (parent == null) ? transform : parent;

        return created;
    }

    private void CreateCeilingMask()
    {
        ceilingMaskTexture = new Texture2D(LevelHelpers.TileCountX, LevelHelpers.TileCountZ);

        ceilingMaskTexture.filterMode = FilterMode.Point;

        UpdateCeilingMask();

        GameObject ceilingMask = GameObject.CreatePrimitive(PrimitiveType.Quad);
        ceilingMask.name = "CeilingMask";
        ceilingMask.transform.parent = transform;
        ceilingMask.transform.position = new Vector3(LevelHelpers.TerrainSizeX / 2f, LevelHelpers.CeilingMaskPositionY, LevelHelpers.TerrainSizeZ / 2f);
        ceilingMask.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        ceilingMask.transform.localScale = new Vector3(LevelHelpers.TerrainSizeX, LevelHelpers.TerrainSizeZ);

        MeshRenderer renderer = ceilingMask.GetComponent<MeshRenderer>();
        renderer.material = CeilingMaskMaterial;
        renderer.material.mainTexture = ceilingMaskTexture;
    }

    private void UpdateCeilingMask()
    {
        for (int z = 0; z < LevelHelpers.TileCountZ; ++z)
        {
            for (int x = 0; x < LevelHelpers.TileCountX; ++x)
            {
                if (Model.HellContiguousTiles[x, z] != null)
                {
                    ceilingMaskTexture.SetPixel(x, z, Color.clear);
                }
                else
                {
                    ceilingMaskTexture.SetPixel(x, z, Color.black);
                }
            }
        }

        ceilingMaskTexture.Apply();
    }
}
