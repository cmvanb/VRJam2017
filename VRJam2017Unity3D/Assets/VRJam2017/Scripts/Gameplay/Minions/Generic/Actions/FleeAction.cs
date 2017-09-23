
using UnityEngine;
using System.Collections;

public class FleeAction : MovementAction
{
    public GameObject GameObject;

    public FleeAction(GameObject owner, GameObject gameObject) : base(owner, gameObject.transform.position)
    {
        GameObject = gameObject;

    }

    public override IEnumerator Perform()
    {
        setDestination(getFleeTarget());

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
        if(GameObject.transform.hasChanged)
        {
            setDestination(getFleeTarget());
        }
    }

    private Vector3 getFleeTarget()
    {
        // Run away from where you are.
        Vector3 direction = Owner.transform.position - GameObject.transform.position;

        return Owner.transform.position + direction;
    }
}