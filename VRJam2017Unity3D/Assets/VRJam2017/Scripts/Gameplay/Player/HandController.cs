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

    private Vector3 pointerDestination;

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
            pointerDestination = args.destinationPosition;
        };

        GetComponent<VRTK_ControllerEvents>().TriggerPressed += OnTriggerPressed;
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += OnTriggerReleased;
        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += OnTouchpadPressed;
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += OnTouchpadReleased;
    }

    // public void Update()
    // {
    //     if (summoner.IsSummoning)
    //     {

    //     }
    // }

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
                    commander.MoveCommand(pointerDestination);

                    // TODO: Consider spawning 'command feedback', like particles or animation.
                }
                else if (commander.CommandMode == PlayerCommander.CommandModes.GUARD)
                {
                    commander.GuardCommand(pointerDestination);

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
    }
}