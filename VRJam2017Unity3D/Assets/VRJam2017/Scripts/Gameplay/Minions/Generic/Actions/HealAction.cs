
using UnityEngine;
using System.Collections;

public class HealAction : UnitAction
{
    public GameObject GameObject;

    public HealAction(GameObject owner, GameObject gameObject) : base(owner)
    {
        GameObject = gameObject;
    }

    public override IEnumerator Perform()
    {
        while(!isDead(GameObject) && !isHealed(GameObject) && isInRange(GameObject))
        {
            Healer healer = Owner.GetComponent<Healer>();

            GameObject.SendMessage("Heal", healer.HealPower);

            yield return new WaitForSeconds(healer.HealRate);
        }

        // TODO: Refactor to extend GameObjectMovementAction like the attack does.
        if(!isDead(GameObject) && !isHealed(GameObject))
        {
            ActionQueue queue = Owner.GetComponent<ActionQueue>();

            queue.Insert(new GameObjectAction(Owner, GameObject));
        }

        Owner.SendMessage("ActionComplete");
    }

    bool isInRange(GameObject other)
    {
        Healer healer = Owner.GetComponent<Healer>();

        float rangeSqr = healer.HealRange * healer.HealRange;

        Vector3 target2d = other.transform.position;
        
        target2d.y = 0;

        Vector3 position2d = Owner.transform.position;

        position2d.y = 0;

        float distanceSqr = (target2d - position2d).sqrMagnitude;

        return distanceSqr < rangeSqr;
    }

    bool isHealed(GameObject other)
    {
        return other.GetComponent<Health>().IsAtMaxHealth();
    }

    
    bool isDead(GameObject other)
    {
        return (other == null || other.GetComponent<Death>().Dead);
    }
}