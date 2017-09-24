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

        // Hide pointer if pointing up (flight), show if pointing down (dash).
        if (hand == Hands.Left
            && controllerEvents.touchpadPressed)
        {
            HidePointerIfPointingUp();
        }

        // Paint dig tiles.
        if (hand == Hands.Left
            && controllerEvents.triggerClicked)
        {
            HidePointerIfPointingUp();

            digger.PaintDig(destination);
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
                // Start attacking.
                attacker.Attack();
            }
            else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            {
                // Start painting dig tiles.
                SetPointerActive(true);

                digger.Dig(destination);
            }
        }
        else if (hand == Hands.Right)
        {
            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                // TODO: implement this check
                bool minionInGrabRange = false;

                if (minionInGrabRange)
                {
                    // TODO: implement - grab minion
                }
                else
                {
                    // Start summoning.
                    SetPointerActive(true);

                    summoner.Summon();
                }
            }
        }
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
                // Stop attacking.
                attacker.StopAttack();
            }
            else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            {
                // Stop painting dig tiles.
                SetPointerActive(false);

                digger.StopDig();
            }
        }
        else if (hand == Hands.Right)
        {
            if (summoner.IsSummoning)
            {
                // Stop summoning.
                SetPointerActive(false);

                summoner.StopSummon();

                // Issue command.
                if (commander.CommandMode == PlayerCommander.CommandModes.MOVE)
                {
                    commander.MoveCommand(destination);
                }
                else if (commander.CommandMode == PlayerCommander.CommandModes.GUARD)
                {
                    commander.GuardCommand(destination);
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
                    && flyer.PositionIsValidLandingZone(destination))
                {
                    flyer.Land(destinationArgs);
                }
            }
        }
    }

    private void SetPointerActive(bool active)
    {
        pointerRenderer.Toggle(active, active);
        pointer.Toggle(active);
    }

    private void HidePointerIfPointingUp()
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

    private bool IsPointerPointingUp()
    {
        if (Vector3.Dot(Vector3.up, transform.forward) > 0.5)
        {
            return true;
        }

        return false;
    }
}