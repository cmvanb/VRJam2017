﻿using System.Collections.Generic;
using UnityEngine;

public static class LevelHelpers
{
    public static float TerrainSizeX
    {
        get
        {
            return LevelController.Instance.Terrain.terrainData.size.x;
        }
    }

    public static float TerrainSizeZ
    {
        get
        {
            return LevelController.Instance.Terrain.terrainData.size.z;
        }
    }

    public static float TileSize = 4f;

    public static int TileCountX
    {
        get
        {
            return (int)(TerrainSizeX / TileSize);
        }
    }

    public static int TileCountZ
    {
        get
        {
            return (int)(TerrainSizeZ / TileSize);
        }
    }

    public static float WallPositionY = 3f;
    public static float CeilingMaskPositionY = 7.01f;
    public static float TileDigMarkerPositionY = 7.05f;

    public static Vector3 WorldPosFromTilePos(int x, int z)
    {
        Vector3 result = new Vector3(x * TileSize, 0f, z * TileSize);

        float y = GetTerrainHeightAtWorldPos(result);

        result = new Vector3(result.x, y, result.z);

        return result;
    }

    public static Vector3 WorldPosFromTilePosSetY(int x, int z, float worldY)
    {
        return new Vector3(x * TileSize, worldY, z * TileSize);
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

    public static List<LevelTile> GetAdjacentTiles(LevelModel model, int xPosition, int zPosition)
    {
        List<LevelTile> tiles = new List<LevelTile>();

        for (int x = (int)xPosition - 1; x <= (int)xPosition + 1; ++x)
        {
            if (!LevelHelpers.TileIsInBounds(model, x, zPosition))
            {
                continue;
            }

            tiles.Add(model.Tiles[x, zPosition]);
        }

        for (int z = (int)zPosition - 1; z <= (int)zPosition + 1; ++z)
        {
            if (!LevelHelpers.TileIsInBounds(model, xPosition, z))
            {
                continue;
            }

            tiles.Add(model.Tiles[xPosition, z]);
        }

        return tiles;
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

    public static bool IsTileAdjacentToDigMarkedTile(LevelModel model, int x, int z)
    {
        List<LevelTile> neighbours = GetSurroundingTiles(model, x, z);

        foreach (LevelTile n in neighbours)
        {
            if (n.MarkedForDigging)
            {
                return true;
            }
        }

        return false;
    }
}
