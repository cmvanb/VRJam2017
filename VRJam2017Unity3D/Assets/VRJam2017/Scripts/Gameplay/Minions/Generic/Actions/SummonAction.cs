using UnityEngine;
using UnityEngine.AI;

public class SummonAction : GameObjectMovementAction
{

    public SummonAction(GameObject owner, GameObject target) : base(owner, target)
    {
        stoppingDistance = 3;
    }
}