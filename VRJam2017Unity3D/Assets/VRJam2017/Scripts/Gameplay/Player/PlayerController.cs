using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float SummonInterval = 0.5f;

    public List<GameObject> SummonedMinions = new List<GameObject>();
    
    private float summonTimer;
    private bool isSummoning = false;

    public void Start()
    {
        // TODO: hook up controller buttons to summon/stopsummon
    }

    public void Summon()
    {
        isSummoning = true;

        summonTimer += Time.deltaTime;

        if (summonTimer > SummonInterval)
        {
            SummonOneMinion();
            summonTimer = 0f;
        }
    }

    public void StopSummon()
    {
        isSummoning = false;
        summonTimer = 0f;
    }

    private void SummonOneMinion()
    {
        // TODO: implement
        //GameObject minion = MinionManager.Summon(transform);

        //SummonedMinions.Add(minion);
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
        throw new NotImplementedException();
    }

    public void StopAttack()
    {
        throw new NotImplementedException();
    }

    public void PaintDig()
    {
        throw new NotImplementedException();
    }
}
