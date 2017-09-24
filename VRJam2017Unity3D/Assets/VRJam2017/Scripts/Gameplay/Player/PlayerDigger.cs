using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerDigger : MonoBehaviour
{
    public float DigMarkerWorldPositionY = 4.25f;

    public bool IsDigging = false;

    public enum PaintModes
    {
        PAINT,
        ERASE
    }
    private PaintModes paintMode = PaintModes.PAINT;

    public void Dig(DestinationMarkerEventArgs args)
    {
        // TODO: improve by using raycast to dig marker height
        Vector3 target = new Vector3(args.target.position.x, DigMarkerWorldPositionY, args.target.position.z);

        Debug.LogWarning("START DIG");
        LevelTile tile = LevelHelpers.GetTileAtWorldPos(LevelController.Instance.Model, target);

        if (TileIsDiggable(tile))
        {
            paintMode = tile.MarkedForDigging ? PaintModes.ERASE : PaintModes.PAINT;

            IsDigging = true;

            PaintDig(args);
        }
    }

    public void StopDig()
    {
        Debug.LogWarning("STOP DIG");
        IsDigging = false;
    }

    public void PaintDig(DestinationMarkerEventArgs args)
    {
        if (!IsDigging)
        {
            return;
        }

        // TODO: improve by using raycast to dig marker height
        Vector3 target = new Vector3(args.target.position.x, DigMarkerWorldPositionY, args.target.position.z);

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

            LevelController.Instance.UpdateTileDigMarker(tile);
            UpdateDigList();
        }
    }

    private bool TileIsDiggable(LevelTile tile)
    {
        // TODO: tile is not opened AND (tile is adjacent to room OR tile is adjacent to highlighted tile)
        return !tile.Opened && (TileIsAdjacentToRoom(tile));
    }

    private bool TileIsAdjacentToRoom(LevelTile tile)
    {
        return true;
    }

    public void UpdateDigList()
    {
        // TODO: implement
    }
}
