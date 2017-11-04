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
        // Debug.LogWarning("MOVE COMMAND");

        // TODO: summoned minions can contain duplicates...
        foreach (GameObject minion in summoner.SummonedMinions)
        {
            if (minion == null)
            {
                continue;
            }

            ActionQueue q = minion.GetComponent<ActionQueue>();

            // if minion's current action is summon, cancel that action
            q.CancelSummon();

            // tell minion to move to target position
            q.InsertBeforeCurrent(new MovementAction(minion, target));
        }

        summoner.SummonedMinions.Clear();

        //minion.GetComponent<ActionQueue>().IsCurrentInteruptable();

        // TODO: Consider spawning 'command feedback', like particles or animation.
    }

    // GUARD
    public void GuardCommand(Vector3 target)
    {
        // Debug.LogWarning("GUARD COMMAND");
        // TODO: Consider spawning 'command feedback', like particles or animation.
    }
}
