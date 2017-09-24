using System.Collections.Generic;
using UnityEngine;

public static class LevelHelpers
{
    public static float TerrainSize = 1024f;
    public static float TileSize = 4f;

    public static int NumTiles
    {
        get
        {
            return (int)(TerrainSize / TileSize);
        }
    }

    public static Vector3 WorldPosFromTilePos(int x, int z)
    {
        // TODO: GET Y FROM TERRAIN HEIGHT
        float y = 0f;

        return new Vector3(x * TileSize, y, z * TileSize);
    }

    public static Vector2 TilePosFromWorldPos(Vector3 position)
    {
        return new Vector2(position.x / TileSize, position.z / TileSize);
    }

    public static bool TileIsInBounds(int x, int z)
    {
        return x >=0 && x < NumTiles && z >=0 && z < NumTiles;
    }

    public static float GetTerrainHeightAtWorldPos(Vector3 worldPosition)
    {
        return LevelController.Instance.Terrain.SampleHeight(worldPosition);
    }

    public static List<LevelTile> GetSurroundingTiles(LevelModel model, int xPosition, int zPosition)
    {
        List<LevelTile> tiles = new List<LevelTile>();

        for (int z = (int)zPosition - 1; z <= (int)zPosition + 1; ++z)
        {
            for (int x = (int)xPosition - 1; x <= (int)xPosition + 1; ++x)
            {
                if (!LevelHelpers.TileIsInBounds(x, z))
                {
                    continue;
                }

                tiles.Add(model.Tiles[x, z]);
            }
        }

        return tiles;
    }
}
