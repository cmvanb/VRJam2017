using UnityEngine;
using VRTK;

public class HandController : MonoBehaviour
{
    public enum Hands
    {
        RIGHT,
        LEFT
    }
    public Hands hand = Hands.RIGHT;

    public GameObject PlayerController;

    public enum PointerTypes
    {
        BEZIER,
        STRAIGHT
    }
    private PointerTypes pointerType = PointerTypes.BEZIER;

    private VRTK_Pointer pointer;
    private VRTK_BezierPointerRenderer bezierRenderer;
    private VRTK_StraightPointerRenderer straightRenderer;
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
        bezierRenderer = GetComponent<VRTK_BezierPointerRenderer>();
        straightRenderer = GetComponent<VRTK_StraightPointerRenderer>();

        attacker = PlayerController.GetComponent<PlayerAttacker>();
        commander = PlayerController.GetComponent<PlayerCommander>();
        digger = PlayerController.GetComponent<PlayerDigger>();
        flyer = PlayerController.GetComponent<PlayerFlyer>();
        mover = PlayerController.GetComponent<PlayerMover>();
        summoner = PlayerController.GetComponent<PlayerSummoner>();

        mover.PointerRenderer = bezierRenderer;

        SetPointer(false);

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
            SetPointer(false);
            return;
        }

        // Hide pointer if pointing up (flight), show if pointing down (dash).
        if (hand == Hands.LEFT
            && controllerEvents.touchpadPressed)
        {
            HidePointerIfPointingUp();
        }

        // Paint dig tiles.
        if (hand == Hands.LEFT
            && controllerEvents.triggerClicked)
        {
            HidePointerIfPointingUp(PointerTypes.STRAIGHT);

            digger.PaintDig(destinationArgs);
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

        if (hand == Hands.LEFT)
        {
            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                // Start attacking.
                attacker.Attack();
            }
            else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            {
                // Start painting dig tiles.
                SetPointer(true, PointerTypes.STRAIGHT);

                digger.Dig(destinationArgs);
            }
        }
        else if (hand == Hands.RIGHT)
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
                    SetPointer(true);

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

        if (hand == Hands.LEFT)
        {
            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                // Stop attacking.
                attacker.StopAttack();
            }
            else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            {
                // Stop painting dig tiles.
                SetPointer(false);

                digger.StopDig();
            }
        }
        else if (hand == Hands.RIGHT)
        {
            if (summoner.IsSummoning)
            {
                // Stop summoning.
                SetPointer(false);

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

        if (hand == Hands.LEFT)
        {
            SetPointer(false);

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

    private void SetPointer(bool active, PointerTypes pt = PointerTypes.BEZIER)
    {
        pointerType = pt;

        pointer.Toggle(active);

        bool bezierActive = active && pt == PointerTypes.BEZIER;
        bezierRenderer.Toggle(bezierActive, bezierActive);

        bool straightActive = active && pt == PointerTypes.STRAIGHT;
        straightRenderer.Toggle(straightActive, straightActive);

        pointer.pointerRenderer = pt == PointerTypes.BEZIER ? (VRTK_BasePointerRenderer)bezierRenderer : (VRTK_BasePointerRenderer)straightRenderer;
    }

    private void HidePointerIfPointingUp(PointerTypes pt = PointerTypes.BEZIER)
    {
        // Hide pointer if pointing up (flight), show if pointing down (dash).
        if (pointer.IsPointerActive() && IsPointerPointingUp())
        {
            SetPointer(false, pt);
        }
        else if (!pointer.IsPointerActive())
        {
            SetPointer(true, pt);
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