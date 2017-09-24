using System.Collections.Generic;
using UnityEngine;

public static class LevelHelpers
{
    public static float TerrainSize = 1024f;
    public static float TileSize = 4f;

    public static Vector3 WorldPosFromTilePos(int x, int z)
    {
        // TODO: GET Y FROM TERRAIN HEIGHT
        Vector3 result = new Vector3(x * TileSize, 0f, z * TileSize);

        float y = GetTerrainHeightAtWorldPos(result);

        result = new Vector3(result.x, y, result.z);

        return result;
    }

    public static Vector2 TilePosFromWorldPos(Vector3 position)
    {
        return new Vector2(position.x / TileSize, position.z / TileSize);
    }

    public static LevelTile GetTileAtWorldPos(LevelModel model, Vector3 position)
    {
        Vector2 tilePos = TilePosFromWorldPos(position);

        int x = (int)tilePos.x;
        int z = (int)tilePos.y;
        
        if (TileIsInBounds(model, x, z))
        {
            return model.Tiles[x, z];
        }

        return null;
    }

    public static bool TileIsInBounds(LevelModel model, int x, int z)
    {
        return x >=0 && x < model.Width && z >=0 && z < model.Length;
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
                if (!LevelHelpers.TileIsInBounds(model, x, z))
                {
                    continue;
                }

                tiles.Add(model.Tiles[x, z]);
            }
        }

        return tiles;
    }

    public static bool IsTileInHell(LevelModel model, int x, int z)
    {
        return model.HellContiguousTiles[x,z] != null;
    }

    public static bool IsTileInHeaven(LevelModel model, int x, int z)
    {
        return model.HeavenContiguousTiles[x,z] != null;
    }

    public static bool IsTileAdjacentToHell(LevelModel model, int x, int z)
    {
        List<LevelTile> neighbours = GetSurroundingTiles(model, x, z);

        foreach (LevelTile n in neighbours)
        {
            if (IsTileInHell(model, n.X, n.Z))
            {
                return true;
            }
        }

        return false;
    }
}
