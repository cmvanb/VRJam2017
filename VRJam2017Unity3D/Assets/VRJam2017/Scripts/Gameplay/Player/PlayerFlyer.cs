using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VRTK;

public class PlayerFlyer : MonoBehaviour
{
    public float FlyingHeight = 20f;
    public float FlightDuration = 1f;
    public float FlyingTeleportDistance = 32f;
    public float FlyingTeleportDuration = 1f;

    // FLIGHT MODE
    public event FlightStateChangedHandler FlightStateChanged;
    public delegate void FlightStateChangedHandler(object sender, FlightStates newState);

    public enum FlightStates
    {
        GROUNDED,
        FLYING
    };
    private FlightStates flightState = FlightStates.GROUNDED;
    public FlightStates FlightState
    {
        get
        {
            return flightState;
        }
        set
        {
            if (flightState != value)
            {
                flightState = value;

                if (FlightStateChanged != null)
                {
                    FlightStateChanged(this, flightState);
                }
            }
        }
    }

    private PlayerMover mover;

    public void Start()
    {
        mover = GetComponent<PlayerMover>();
    }

    public void Fly()
    {
        if (flightState != FlightStates.GROUNDED)
        {
            return;
        }

        // Debug.LogWarning("FLY");

        flightState = FlightStates.FLYING;

        Transform playArea = GameManager.Instance.PlayArea;

        Vector3 flyingPosition = playArea.position + new Vector3(0f, FlyingHeight, 0f);

        playArea.DOMove(flyingPosition, FlightDuration).SetEase(Ease.InOutQuad);

        mover.SetTeleportDistance(FlyingTeleportDistance);
    }

    public void Land(DestinationMarkerEventArgs dashArgs)
    {
        if (flightState != FlightStates.FLYING)
        {
            return;
        }

        if (dashArgs.target == null)
        {
            Debug.LogError("dashArgs target is null, can't dash");
        }

        Transform playArea = GameManager.Instance.PlayArea;

        playArea.DOKill();

        // Debug.LogWarning("LAND");

        flightState = FlightStates.GROUNDED;

        playArea.DOMove(dashArgs.destinationPosition, FlightDuration).SetEase(Ease.InOutQuad);

        //mover.Dash(dashArgs, FlyingTeleportDuration);

        mover.SetTeleportDistance(mover.GroundTeleportDistance);
    }

    public bool PositionIsValidLandingZone(Vector3 destination)
    {
        Vector2 tilePos = LevelHelpers.TilePosFromWorldPos(destination);

        LevelModel model = LevelController.Instance.Model;

        int x = (int)tilePos.x;
        int z = (int)tilePos.y;

        if (!LevelHelpers.TileIsInBounds(model, x, z))
        {
            return false;
        }

        LevelTile tile = model.Tiles[x, z];

        if (tile.Opened)
        {
            return true;
        }

        return false;
    }
}
