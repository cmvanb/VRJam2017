
using UnityEngine;
using System.Collections;

public class CaptureAction : GameObjectMovementAction
{
    GameObject captive;

    public CaptureAction(GameObject owner, GameObject gameObject, GameObject captive) : base(owner, gameObject)
    {
        this.captive = captive;
    }

    public override IEnumerator Perform()
    {
        captive.SendMessage("Capture");

        Transform oldParent = captive.transform.parent;

        captive.transform.parent = Owner.transform;

        setDestination(Target.transform.position);

        while(!hasReachedTarget())
        {
            updatePosition();

            yield return 0;
        }

        Target.SendMessage("Release", captive, SendMessageOptions.DontRequireReceiver);

        captive.SendMessage("Release", SendMessageOptions.DontRequireReceiver);

        captive.transform.parent = oldParent;


        Stop();

        Owner.SendMessage("ActionComplete");
    }
}