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

    private VRTK_Pointer pointer;
    private VRTK_BasePointerRenderer pointerRenderer;

    public void Start()
    {
        pointer = GetComponent<VRTK_Pointer>();
        pointerRenderer = GetComponent<VRTK_BasePointerRenderer>();

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

        PlayerController player = PlayerController.Instance;

        if (hand == Hands.Left)
        {
            if (player.FlightState == PlayerController.FlightStates.GROUNDED)
            {
                player.Attack();
            }
            else if (player.FlightState == PlayerController.FlightStates.FLYING)
            {
                player.PaintDig();
            }
        }
        else if (hand == Hands.Right)
        {
            if (player.FlightState == PlayerController.FlightStates.GROUNDED)
            {
                // TODO: implement
                // if minion is within reach, grab that minion
                // else, summon

                player.Summon();
            }
            // TODO: Consider enabling this.
            // else if (player.FlightState == PlayerController.FlightStates.FLYING)
            // {
            //     player.PaintDig();
            // }
        }
    }

    private void OnTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log(hand.ToString() + " trigger released");

        PlayerController player = PlayerController.Instance;

        if (hand == Hands.Left)
        {
            if (player.FlightState == PlayerController.FlightStates.GROUNDED)
            {
                player.StopAttack();
            }
        }
        else if (hand == Hands.Right)
        {
            if (player.IsSummoning)
            {
                player.StopSummon();

                if (player.CommandMode == PlayerController.CommandModes.MOVE)
                {
                    // TODO: Get target from pointer.
                    Vector3 target = Vector3.zero;

                    player.MoveCommand(target);
                }
                else if (player.CommandMode == PlayerController.CommandModes.GUARD)
                {
                    player.GuardCommand();
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