
using UnityEngine;
using System.Collections;

public class GameObjectMovementAction : MovementAction
{
    public GameObject Target;


    public GameObjectMovementAction(GameObject owner, GameObject target) : base(owner, target.transform.position)
    {
        Target = target;
    }

    public override IEnumerator Perform()
    {
        setDestination(Target.transform.position);

        while(!hasReachedTarget())
        {
            updatePosition();

            yield return 0;
        }

        Stop();

        Owner.SendMessage("ActionComplete");
    }

    protected void updatePosition()
    {
        if(Target.transform.hasChanged)
        {
            setDestination(Target.transform.position);
        }
    }
}