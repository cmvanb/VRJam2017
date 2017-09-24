using UnityEngine;
using System.Collections.Generic;

public class LevelModel
{
    public int Width;
    public int Length;
    public LevelTile[,] Tiles;

    public int SpawnHoleSize = 13;
    public int MinHoleSize = 3;
    public int MaxHoleSize = 9;
    public float HoleProbability = 0.01f;

    public LevelTile[,] HellContiguousTiles;

    public LevelTile[,] HeavenContiguousTiles;

    public Vector2 HellSpawn;

    public Vector2 HeavenSpawn;

    public List<Vector3> Rooms;

    public LevelModel(int width, int length)
    {
        Width = width;
        Length = length;

        Rooms = new List<Vector3>();

        Tiles = new LevelTile[width, length];

        HellContiguousTiles = new LevelTile[width, length];

        HeavenContiguousTiles = new LevelTile[width, length];

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

                    Vector2 roomCenter = new Vector3(x, z, holeSize);

                    PunchHole(roomCenter, holeSize);

                    Rooms.Add(roomCenter);
                }
            }
        }

        // Punch hole for starting location.
        HellSpawn.Set(width/2, length/2);

        PunchHole(HellSpawn, SpawnHoleSize);

        var angle = Random.value*Mathf.PI*2;

        HeavenSpawn.x = (int)(width/2 + Mathf.Cos(angle)*(width - SpawnHoleSize)/2);

        HeavenSpawn.y = (int)(length/2 + Mathf.Sin(angle)*(length - SpawnHoleSize)/2);

        PunchHole(HeavenSpawn, SpawnHoleSize);

        // Evaluate tiles for wall types
        for (int z = 0; z < length; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                EvaluateTile(x, z);
            }
        }

        FloodFill((int)HellSpawn.x, (int)HellSpawn.y, HellContiguousTiles);
        FloodFill((int)HeavenSpawn.x, (int)HeavenSpawn.y, HeavenContiguousTiles);
    }

    public LevelTile HellSpawnTile
    {
        get { return Tiles[(int)HellSpawn.x, (int)HellSpawn.y]; }
    }

    public LevelTile HeavenSpawnTile
    {
        get { return Tiles[(int)HeavenSpawn.x, (int)HeavenSpawn.y]; }
    }


    public void UpdateContiguousTilesFrom(int x, int z)
    {
        List<LevelTile> surroundingTiles = LevelHelpers.GetSurroundingTiles(this, x, z);

        LevelTile foundHell = surroundingTiles.Find(tile => HellContiguousTiles[tile.X, tile.Z] != null);

        LevelTile foundHeaven = surroundingTiles.Find(tile => HeavenContiguousTiles[tile.X, tile.Z] != null);

        // if we're about to join, just copy everything to save a painful flood fill.
        if(foundHell != null && foundHeaven != null)
        {
            for (int iz = 0; iz < Length; ++iz)
            {
                for (int ix = 0; ix < Width; ++ix)
                {
                    if(HellContiguousTiles[ix, iz] != null)
                    {
                        HeavenContiguousTiles[ix, iz] = HellContiguousTiles[ix, iz];
                    }
                    else if(HeavenContiguousTiles[ix, iz] != null)
                    {
                        HellContiguousTiles[ix, iz] = HeavenContiguousTiles[ix, iz];
                    }
                }
            }
        }
        
        if(foundHell != null)
        {
            FloodFill(x,z, HellContiguousTiles);
        }
        
        if(foundHeaven != null)
        {
            FloodFill(x,z, HeavenContiguousTiles);
        }
    }
    
    void FloodFill(int x, int z, LevelTile[,] tileList)
    {
        if(tileList[x,z] == null && Tiles[x, z].Opened)
        {
            tileList[x,z] = Tiles[x,z];

            List<LevelTile> surroundingTiles = LevelHelpers.GetSurroundingTiles(this, x, z);

            surroundingTiles.ForEach(tile => FloodFill(tile.X, tile.Z, tileList));
        }
        
    }

    // private int[] WallT = new int[9] {
    //     2, 1, 2,
    //     2, 0, 2,
    //     2, 2, 2
    // };

    // private int[] WallR = new int[9] {
    //     2, 2, 2,
    //     2, 0, 1,
    //     2, 2, 2
    // };

    // private int[] WallB = new int[9] {
    //     2, 2, 2,
    //     2, 0, 2,
    //     2, 1, 2
    // };

    // private int[] WallL = new int[9] {
    //     2, 2, 2,
    //     1, 0, 2,
    //     2, 2, 2
    // };

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
            tile.WallTop = tile.WallBottom = tile.WallLeft = tile.WallRight = false;
            return;
        }

        // If the tile position is out of bounds, value is 2 (ignore tile).
        // If tile is inside bounds, opened = 1, closed = 0
        //int tileTL = (x > 0 && z > 0)                  ? (Tiles[x - 1, z - 1].Opened ? 1 : 0) : 2;
        int tileTT = z > 0                             ? (Tiles[x, z - 1].Opened     ? 1 : 0) : 2;
        // int tileTR = (x < Width - 1 && z > 0)          ? (Tiles[x + 1, z - 1].Opened ? 1 : 0) : 2;
        int tileRR = x < Width - 1                     ? (Tiles[x + 1, z].Opened     ? 1 : 0) : 2;
        // int tileBR = (x < Width - 1 && z < Length - 1) ? (Tiles[x + 1, z + 1].Opened ? 1 : 0) : 2;
        int tileBB = z < Length - 1                    ? (Tiles[x, z + 1].Opened     ? 1 : 0) : 2;
        // int tileBL = (x > 0 && z < Length - 1)         ? (Tiles[x - 1, z + 1].Opened ? 1 : 0) : 2;
        int tileLL = x > 0                             ? (Tiles[x - 1, z].Opened     ? 1 : 0) : 2;

        if (tileTT == 1)
        {
            tile.WallTop = true;
        }
        if (tileRR == 1)
        {
            tile.WallRight = true;
        }
        if (tileBB == 1)
        {
            tile.WallBottom = true;
        }
        if (tileLL == 1)
        {
            tile.WallLeft = true;
        }

        // int[] openedTable = new int[9] { 
        //     tileTL, tileTT, tileTR,
        //     tileLL, 0, tileRR,
        //     tileBL, tileBB, tileBR
        // };

        // if (CheckTables(openedTable, WallT))
        // {
        //     tile.WallTop = true;
        // }
        // if (CheckTables(openedTable, WallR))
        // {
        //     tile.WallRight = true;
        // }
        // if (CheckTables(openedTable, WallB))
        // {
        //     tile.WallBottom = true;
        // }
        // if (CheckTables(openedTable, WallL))
        // {
        //     tile.WallLeft = true;
        // }
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
            else if (evaluationTable[i] == openedTable[i])
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
                if (!LevelHelpers.TileIsInBounds(this, x, z))
                {
                    continue;
                }

                if (Vector2.Distance(origin, new Vector2(x, z)) < radius)
                {
                    Tiles[x, z].Opened = true;
                }
            }
        }
    }
}
