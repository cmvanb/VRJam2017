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
    private VRTK_BasePointerRenderer pointerRenderer;
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
        pointerRenderer = GetComponent<VRTK_BasePointerRenderer>();
        attacker = PlayerController.GetComponent<PlayerAttacker>();
        commander = PlayerController.GetComponent<PlayerCommander>();
        digger = PlayerController.GetComponent<PlayerDigger>();
        flyer = PlayerController.GetComponent<PlayerFlyer>();
        mover = PlayerController.GetComponent<PlayerMover>();
        summoner = PlayerController.GetComponent<PlayerSummoner>();

        SetPointerActive(false);

        pointer.DestinationMarkerExit += (object marker, DestinationMarkerEventArgs args) => {
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
        if (controllerEvents.touchpadPressed
            && hand == Hands.Left
            && flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
        {
            // Hide pointer if pointing up (flight), show if pointing down (dash).
            SetPointerActive(!IsPointerPointingUp());
        }
    }

    private void OnTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log(hand.ToString() + " trigger pressed");

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
        Debug.Log(hand.ToString() + " touchpad pressed");
    }

    private void OnTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log(hand.ToString() + " touchpad released");

        if (hand == Hands.Left)
        {
            if (flyer.FlightState == PlayerFlyer.FlightStates.GROUNDED)
            {
                SetPointerActive(false);

                if (IsPointerPointingUp())
                {
                    flyer.ToggleFlightMode();
                }
                else
                {
                    mover.Dash(destinationArgs);
                }
            }
        }
    }

    private bool IsPointerPointingUp()
    {
        // TODO: calculate this based on pointer vector
        return false;
    }
}