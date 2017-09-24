using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SummonAction : GameObjectMovementAction
{

    public SummonAction(GameObject owner, GameObject target) : base(owner, target)
    {
        stoppingDistance = 3;
    }

    public override IEnumerator Perform()
    {
        setDestination(Position);

        while(true)
        {
            if(hasReachedTarget())
            {
                Stop();
            }
            else
            {
                setDestination(Position);
            }

            yield return 0;     
        }

        Owner.SendMessage("ActionComplete");
    }
}