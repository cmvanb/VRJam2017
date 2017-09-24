using UnityEngine;
using VRTK;

public class HandController : MonoBehaviour
{
    public enum Hands
    {
        Right,
        Left
    }
    public Hands hand = Hands.Right;

    public GameObject PlayerController;

    private VRTK_Pointer pointer;
    private VRTK_BezierPointerRenderer pointerRenderer;
    private PlayerAttacker attacker;
    private PlayerCommander commander;
    private PlayerDigger digger;
    private PlayerFlyer flyer;
    private PlayerMover mover;
    private PlayerSummoner summoner;
    // TODO: pick up minion

    private VRTK_ControllerEvents controllerEvents;
    private DestinationMarkerEventArgs destinationArgs;
    private Vector3 destination;

    public void Start()
    {
        pointer = GetComponent<VRTK_Pointer>();
        pointerRenderer = GetComponent<VRTK_BezierPointerRenderer>();
        attacker = PlayerController.GetComponent<PlayerAttacker>();
        commander = PlayerController.GetComponent<PlayerCommander>();
        digger = PlayerController.GetComponent<PlayerDigger>();
        flyer = PlayerController.GetComponent<PlayerFlyer>();
        mover = PlayerController.GetComponent<PlayerMover>();
        summoner = PlayerController.GetComponent<PlayerSummoner>();

        mover.PointerRenderer = pointerRenderer;

        SetPointerActive(false);

        pointer.DestinationMarkerHover += (object marker, DestinationMarkerEventArgs args) => {
            destinationArgs = args;
            destination = args.destinationPosition;
        };

        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        controllerEvents.TriggerPressed += OnTriggerPressed;
        controllerEvents.TriggerReleased += OnTriggerReleased;
        controllerEvents.TouchpadPressed += OnTouchpadPressed;
        controllerEvents.TouchpadReleased += OnTouchpadReleased;
    }

    public void Update()
    {
        if (mover.IsTeleporting)
        {
            SetPointerActive(false);
            return;
        }

        if (hand == Hands.Left
            && controllerEvents.touchpadPressed)
        {
            // Hide pointer if pointing up (flight), show if pointing down (dash).
            if (pointer.IsPointerActive() && IsPointerPointingUp())
            {
                SetPointerActive(false);
            }
            else if (!pointer.IsPointerActive())
            {
                SetPointerActive(true);
            }
        }
    }

    private void OnTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log(hand.ToString() + " trigger pressed");

        // Can't do stuff while teleporting.
        if (mover.IsTeleporting)
        {
            return;
        }

        if (hand == Hands.Left)
        {
            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                attacker.Attack();
            }
            else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            {
                // TODO: implement
                // if pointing at unhighlighted tile, use digger.CancelDigCommand();
                // otherwise, dig

                digger.DigCommand();
            }
        }
        else if (hand == Hands.Right)
        {
            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                // TODO: implement
                // if minion is within reach, grab that minion
                // else, summon

                SetPointerActive(true);

                summoner.Summon();
            }
            // TODO: Consider enabling this.
            // else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            // {
            //     digger.DigCommand();
            // }
        }
    }

    private void SetPointerActive(bool active)
    {
        pointerRenderer.Toggle(active, active);
        pointer.Toggle(active);
    }

    private void OnTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        // Can't do stuff while teleporting.
        if (mover.IsTeleporting)
        {
            return;
        }

        Debug.Log(hand.ToString() + " trigger released");

        if (hand == Hands.Left)
        {
            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                attacker.StopAttack();
            }
        }
        else if (hand == Hands.Right)
        {
            if (summoner.IsSummoning)
            {
                SetPointerActive(false);
                summoner.StopSummon();

                if (commander.CommandMode == PlayerCommander.CommandModes.MOVE)
                {
                    commander.MoveCommand(destination);

                    // TODO: Consider spawning 'command feedback', like particles or animation.
                }
                else if (commander.CommandMode == PlayerCommander.CommandModes.GUARD)
                {
                    commander.GuardCommand(destination);

                    // TODO: Consider spawning 'command feedback', like particles or animation.
                }
            }
        }
    }

    private void OnTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        // Can't do stuff while teleporting.
        if (mover.IsTeleporting)
        {
            return;
        }

        Debug.Log(hand.ToString() + " touchpad pressed");
    }

    private void OnTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        // Can't do stuff while teleporting.
        if (mover.IsTeleporting)
        {
            return;
        }

        Debug.Log(hand.ToString() + " touchpad released");

        if (hand == Hands.Left)
        {
            SetPointerActive(false);

            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                if (IsPointerPointingUp())
                {
                    flyer.Fly();
                }
                else
                {
                    mover.Dash(destinationArgs, mover.GroundTeleportDuration);
                }
            }
            else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            {
                if (!IsPointerPointingUp()
                    && PositionIsValidLandingZone(destination))
                {
                    flyer.Land(destinationArgs);
                }
            }
        }
    }

    private bool IsPointerPointingUp()
    {
        if (Vector3.Dot(Vector3.up, transform.forward) > 0.5)
        {
            return true;
        }

        return false;
    }

    private bool PositionIsValidLandingZone(Vector3 destination)
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