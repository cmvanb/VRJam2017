
using UnityEngine;
using System.Collections;

public class GameObjectAction : UnitAction
{
    public GameObject GameObject;

    public GameObjectAction(GameObject owner, GameObject gameObject) : base(owner)
    {
        GameObject = gameObject;

    }

    public override IEnumerator Perform()
    {
        if(!isDead(GameObject))
        {
            if(GameObject.GetComponent<Owner>() != null && GameObject.GetComponent<Faction>() != null)
            {
                bool sameOwner = GameObject.GetComponent<Owner>().CurrentOwner == Owner.GetComponent<Owner>().CurrentOwner;

                bool sameFaction = GameObject.GetComponent<Faction>().CurrentFaction == Owner.GetComponent<Faction>().CurrentFaction;

                if(sameOwner || sameFaction)
                {
                    Owner.GetComponent<ActionQueue>().Insert(new GameObjectMovementAction(Owner, GameObject));
                }
                else if(!sameFaction)
                {
                    Owner.GetComponent<ActionQueue>().Insert(new AttackAction(Owner, GameObject));
                }
            }
            else
            {
                Owner.GetComponent<ActionQueue>().Insert(new MovementAction(Owner, GameObject.transform.position));
            }
        }

        Owner.SendMessage("ActionComplete");

        yield return 0;
    }

    bool isDead(GameObject other)
    {
        return (other == null || other.GetComponent<Death>().Dead);
    }
}