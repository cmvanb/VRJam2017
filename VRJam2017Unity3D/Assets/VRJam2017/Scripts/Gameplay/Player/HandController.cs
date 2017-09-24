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
    // paint dig
    // pick up minion

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

        if (hand == Hands.Left)
        {
            pointer.enabled = true;
            pointerRenderer.enabled = true;
        }
        else if (hand == Hands.Right)
        {
            pointer.enabled = false;
            pointerRenderer.enabled = false;
        }

        GetComponent<VRTK_ControllerEvents>().TriggerPressed += OnTriggerPressed;
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += OnTriggerReleased;
        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += OnTouchpadPressed;
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += OnTouchpadReleased;
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

                summoner.Summon();
            }
            // TODO: Consider enabling this.
            // else if (flyer.FlightState == PlayerFlyer.FlightStates.FLYING)
            // {
            //     digger.DigCommand();
            // }
        }
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
                summoner.StopSummon();

                if (commander.CommandMode == PlayerCommander.CommandModes.MOVE)
                {
                    // TODO: Get target from pointer.
                    Vector3 target = Vector3.zero;

                    commander.MoveCommand(target);
                }
                else if (commander.CommandMode == PlayerCommander.CommandModes.GUARD)
                {
                    // TODO: Get target from pointer.
                    Vector3 target = Vector3.zero;

                    commander.GuardCommand(target);
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