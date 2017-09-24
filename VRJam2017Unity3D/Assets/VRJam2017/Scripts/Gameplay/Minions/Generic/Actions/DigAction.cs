
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class DigAction : UnitAction
{
    Vector2 digPosition;

    NavMeshAgent agent;

    public DigAction(GameObject owner, Vector2 position) : base(owner)
    {
        digPosition = position;

        agent = owner.GetComponent<NavMeshAgent>();
    }

    public override IEnumerator Perform()
    {
        Owner.SendMessage("Dig", SendMessageOptions.DontRequireReceiver);

        Digger digger = Owner.GetComponent<Digger>();

        LevelController.Instance.Dig((int)digPosition.x, (int)digPosition.y);

        yield return new WaitForSeconds(digger.DigTime);

        Owner.SendMessage("ActionComplete");
    }
}