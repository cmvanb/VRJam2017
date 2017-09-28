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
        Vector3 target = new Vector3(args.destinationPosition.x, DigMarkerWorldPositionY, args.destinationPosition.z);

        // Debug.LogWarning("START DIG");

        IsDigging = true;

        LevelTile tile = LevelHelpers.GetTileAtWorldPos(LevelController.Instance.Model, target);

        paintMode = tile.MarkedForDigging ? PaintModes.ERASE : PaintModes.PAINT;

        if (TileIsDiggable(tile))
        {
            PaintDig(args);
        }
    }

    public void StopDig()
    {
        // Debug.LogWarning("STOP DIG");
        IsDigging = false;
    }

    public void PaintDig(DestinationMarkerEventArgs args)
    {
        if (!IsDigging)
        {
            return;
        }

        // TODO: improve by using raycast to dig marker height
        Vector3 target = new Vector3(args.destinationPosition.x, DigMarkerWorldPositionY, args.destinationPosition.z);

        LevelTile tile = LevelHelpers.GetTileAtWorldPos(LevelController.Instance.Model, target);

        if (TileIsDiggable(tile))
        {
            if (paintMode == PaintModes.ERASE)
            {
                tile.MarkedForDigging = false;
                MinionManager.Instance.RemoveDigTile(tile);
            }
            else if (paintMode == PaintModes.PAINT)
            {
                tile.MarkedForDigging = true;
                MinionManager.Instance.AddDigTile(tile);
            }

            LevelController.Instance.UpdateTileDigMarker(tile);
        }
    }

    private bool TileIsDiggable(LevelTile tile)
    {
        // tile is not opened AND (tile is adjacent to room OR tile is adjacent to highlighted tile)
        return !tile.Opened
            && (LevelHelpers.IsTileAdjacentToDigMarkedTile(LevelController.Instance.Model, tile.X, tile.Z)
                || LevelHelpers.IsTileAdjacentToHell(LevelController.Instance.Model, tile.X, tile.Z));
    }
}
