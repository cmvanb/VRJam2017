using UnityEngine;
using System.Collections.Generic;

public class LevelModel
{
    public int Width;
    public int Length;
    public LevelTile[,] Tiles;

    public int MinHoleSize = 3;
    public int MaxHoleSize = 9;
    public float HoleProbability = 0.01f;

    public LevelModel(int width, int length)
    {
        Width = width;
        Length = length;

        Tiles = new LevelTile[width, length];

        // Populate tiles.
        for (int z = 0; z < length; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                LevelTile tile = new LevelTile();

                tile.X = x;
                tile.Z = z;

                Tiles[x, z] = tile;
            }
        }

        // Punch random holes.
        for (int z = 0; z < length; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                if (Random.value < HoleProbability)
                {
                    int holeSize = Random.Range(MinHoleSize, MaxHoleSize);

                    PunchHole(new Vector2(x, z), holeSize);
                }
            }
        }

        // Evaluate tiles for wall types
        for (int z = 0; z < length; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                EvaluateTile(x, z);
            }
        }
    }

    private int[] WallT = new int[9] {
        2, 1, 2,
        2, 0, 2,
        2, 2, 2
    };

    private int[] WallR = new int[9] {
        2, 2, 2,
        2, 0, 1,
        2, 2, 2
    };

    private int[] WallB = new int[9] {
        2, 2, 2,
        2, 0, 2,
        2, 1, 2
    };

    private int[] WallL = new int[9] {
        2, 2, 2,
        1, 0, 2,
        2, 2, 2
    };

    // private int[] CorTL = new int[9] {
    //     1, 0, 2,
    //     0, 0, 2,
    //     2, 2, 2
    // };

    public void EvaluateTile(int x, int z)
    {
        LevelTile tile = Tiles[x, z];

        if (tile.Opened)
        {
            return;
        }

        // If the tile position is out of bounds, value is 2 (ignore tile).
        // If tile is inside bounds, opened = 1, closed = 0
        int tileTL = (x > 0 && z > 0)                  ? (Tiles[x - 1, z - 1].Opened ? 1 : 0) : 2;
        int tileTT = z > 0                             ? (Tiles[x, z - 1].Opened     ? 1 : 0) : 2;
        int tileTR = (x < Width - 1 && z > 0)          ? (Tiles[x + 1, z - 1].Opened ? 1 : 0) : 2;
        int tileRR = x < Width - 1                     ? (Tiles[x + 1, z].Opened     ? 1 : 0) : 2;
        int tileBR = (x < Width - 1 && z < Length - 1) ? (Tiles[x + 1, z + 1].Opened ? 1 : 0) : 2;
        int tileBB = z < Length - 1                    ? (Tiles[x, z + 1].Opened     ? 1 : 0) : 2;
        int tileBL = (x > 0 && z < Length - 1)         ? (Tiles[x - 1, z + 1].Opened ? 1 : 0) : 2;
        int tileLL = x > 0                             ? (Tiles[x - 1, z].Opened     ? 1 : 0) : 2;

        int[] openedTable = new int[9] { 
            tileTL, tileTT, tileTR,
            tileLL, 0, tileRR,
            tileBL, tileBB, tileBR
        };

        if (CheckTables(openedTable, WallT))
        {
            tile.WallTop = true;
        }
        if (CheckTables(openedTable, WallR))
        {
            tile.WallRight = true;
        }
        if (CheckTables(openedTable, WallB))
        {
            tile.WallBottom = true;
        }
        if (CheckTables(openedTable, WallL))
        {
            tile.WallLeft = true;
        }
    }

    private bool CheckTables(int[] openedTable, int[] evaluationTable)
    {
        bool atLeastOneMatch = false;

        for (int i = 0; i < 9; ++i)
        {
            // Skip over ignored tiles.
            if (evaluationTable[i] == 2 || openedTable[i] == 2)
            {
                continue;
            }

            if (evaluationTable[i] != openedTable[i])
            {
                return false;
            }
            else if (evaluationTable[i] == 1)
            {
                atLeastOneMatch = true;
            }
        }

        return atLeastOneMatch;
    }

    public void PunchHole(Vector2 origin, int size)
    {
        float radius = (float)size / 2f;
        int halfSize = Mathf.CeilToInt(radius);

        for (int z = (int)origin.y - halfSize; z < (int)origin.y + halfSize; ++z)
        {
            for (int x = (int)origin.x - halfSize; x < (int)origin.x + halfSize; ++x)
            {
                if (!LevelHelpers.TileIsInBounds(x, z))
                {
                    continue;
                }

                Vector2 point = new Vector2(x, z);

                if (Vector2.Distance(origin, point) < radius)
                {
                    Tiles[x, z].Opened = true;
                }
            }
        }
    }
}
