using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : MonoBehaviour 
{
    public enum CommandModes
    {
        MOVE,
        GUARD
    };
    [HideInInspector]
    public CommandModes CommandMode = CommandModes.MOVE;

    private PlayerSummoner summoner;

    public void Start()
    {
        summoner = GetComponent<PlayerSummoner>();
    }

    // MOVE
    public void MoveCommand(Vector3 target)
    {
        Debug.LogWarning("MOVE COMMAND");

        foreach (GameObject minion in summoner.SummonedMinions)
        {
            minion.GetComponent<ActionQueue>().InsertBeforeCurrent(new MovementAction(minion, target));

            // TODO: implement
            // get component
            // if still alive
            // if minion's current action is summon, pop that action
            // tell minion to move to target position
        }

        summoner.SummonedMinions.Clear();

        //minion.GetComponent<ActionQueue>().IsCurrentInteruptable();
        //minion.GetComponent<ActionQueue>().CancelSummon();
    }

    // GUARD
    public void GuardCommand(Vector3 target)
    {
        Debug.LogWarning("GUARD COMMAND");
    }
}
