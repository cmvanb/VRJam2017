﻿using UnityEngine;
using System.Collections.Generic;

public class LevelTile
{
    public bool Opened = false;
    public bool MarkedForDigging = false;
    public int X;
    public int Z;

    public bool WallTop = false;
    public bool WallRight = false;
    public bool WallBottom = false;
    public bool WallLeft = false;

    public List<GameObject> Walls = new List<GameObject>();
    public GameObject DigMarker = null;

    public override string ToString()
    {
        return "[" + X + ", " + Z + "]";
    }
}