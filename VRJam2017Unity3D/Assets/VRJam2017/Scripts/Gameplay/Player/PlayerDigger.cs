using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDigger : MonoBehaviour
{
    public bool IsDigging = false;

    public enum PaintModes
    {
        PAINT,
        ERASE
    }
    private PaintModes paintMode = PaintModes.PAINT;

    public void Dig(Vector3 target)
    {
        Debug.LogWarning("START DIG");
        LevelTile tile = LevelHelpers.GetTileAtWorldPos(LevelController.Instance.Model, target);

        if (TileIsDiggable(tile))
        {
            paintMode = tile.MarkedForDigging ? PaintModes.ERASE : PaintModes.PAINT;

            IsDigging = true;

            PaintDig(target);
        }
    }

    public void StopDig()
    {
        Debug.LogWarning("STOP DIG");
        IsDigging = false;
    }

    public void PaintDig(Vector3 target)
    {
        if (!IsDigging)
        {
            return;
        }

        Debug.Log("1");
        LevelTile tile = LevelHelpers.GetTileAtWorldPos(LevelController.Instance.Model, target);

        if (TileIsDiggable(tile))
        {
            if (paintMode == PaintModes.ERASE)
            {
                tile.MarkedForDigging = false;
            }
            else if (paintMode == PaintModes.PAINT)
            {
                tile.MarkedForDigging = true;
            }

            Debug.Log("2");
            LevelController.Instance.UpdateTileDigMarker(tile);
        }
    }

    private bool TileIsDiggable(LevelTile tile)
    {
        // TODO: tile is not opened AND (tile is adjacent to room OR tile is adjacent to highlighted tile)
        return !tile.Opened;
    }
}
