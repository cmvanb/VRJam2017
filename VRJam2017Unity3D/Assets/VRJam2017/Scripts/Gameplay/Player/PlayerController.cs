using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;

public class PlayerController : MonoSingleton<PlayerController>
{
    public float SummonInterval = 0.5f;

    public enum FlightStates
    {
        GROUNDED,
        FLYING
    };
    [HideInInspector]
    public FlightStates FlightState = FlightStates.GROUNDED;

    public enum CommandModes
    {
        MOVE,
        GUARD
    };
    [HideInInspector]
    public CommandModes CommandMode = CommandModes.MOVE;

    [HideInInspector]
    public List<GameObject> SummonedMinions = new List<GameObject>();
    [HideInInspector]
    public bool IsSummoning = false;

    [HideInInspector]
    public bool IsAttacking = false;

    private float summonTimer = 0f;

    public void Start()
    {
        // TODO: hook up controller buttons to summon/stopsummon
    }

    public void Summon()
    {
        Debug.LogWarning("SUMMON");
        IsSummoning = true;

        // summonTimer += Time.deltaTime;

        // if (summonTimer > SummonInterval)
        // {
        //     SummonOneMinion();
        //     summonTimer = 0f;
        // }

        StartCoroutine(DelayedSummon());
    }

    private IEnumerator DelayedSummon()
    {
        while (IsSummoning)
        {
            yield return new WaitForSeconds(SummonInterval);

            SummonOneMinion();
        }

        yield return null;
    }

    public void StopSummon()
    {
        StopAllCoroutines();

        Debug.LogWarning("STOP SUMMON");

        IsSummoning = false;
        summonTimer = 0f;
    }

    private void SummonOneMinion()
    {
        Debug.Log("summon minion " + Time.time);

        GameObject minion = MinionManager.Instance.SummonOne(gameObject);
        SummonedMinions.Add(minion);

        //minion.GetComponent<ActionQueue>().IsCurrentInteruptable();
        //minion.GetComponent<ActionQueue>().CancelSummon();
    }

    public void MoveCommand(Vector3 target)
    {
        foreach (GameObject minion in SummonedMinions)
        {
            // TODO: implement
            // get component
            // if still alive
            // if minion's current action is summon, pop that action
        }

        SummonedMinions.Clear();
    }

    public void GuardCommand()
    {
        throw new NotImplementedException();
    }

    public void Dash()
    {
        throw new NotImplementedException();
    }

    public void ToggleFlightMode()
    {
        throw new NotImplementedException();
    }

    public void PickupMinion(GameObject minion)
    {
        throw new NotImplementedException();
    }

    public void Attack()
    {
        Debug.LogWarning("ATTACK");

        IsAttacking = true;
    }

    public void StopAttack()
    {
        Debug.LogWarning("STOP ATTACK");

        IsAttacking = false;
    }

    public void PaintDig()
    {
        throw new NotImplementedException();
    }
}
