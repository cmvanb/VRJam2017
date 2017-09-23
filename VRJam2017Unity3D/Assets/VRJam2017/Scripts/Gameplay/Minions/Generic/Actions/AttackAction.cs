
using UnityEngine;
using System.Collections;

public class AttackAction : GameObjectMovementAction
{
    public AttackAction(GameObject owner, GameObject gameObject) : base(owner, gameObject)
    {
    }

    public override IEnumerator Perform()
    {
        if(!isDead(Target))
        {
            setDestination(Target.transform.position);
        }

        while(!isDead(Target))
        {
            updatePosition();

            if(isInRange(Target))
            {
                Stop();

                Attack attack = Owner.GetComponent<Attack>();

                Owner.SendMessage("Attack", SendMessageOptions.DontRequireReceiver);

                Target.SendMessage("WantsToDamage", attack.AttackPower);

                yield return new WaitForSeconds(attack.FireRate);
            }
            else
            {
                yield return 0;
            }
        }

        Stop();

        Owner.SendMessage("ActionComplete");
    }

    bool isInRange(GameObject other)
    {
        Attack attack = Owner.GetComponent<Attack>();

        float rangeSqr = attack.AttackRange * attack.AttackRange;

        Vector3 target2d = other.transform.position;
        
        target2d.y = 0;

        Vector3 position2d = Owner.transform.position;

        position2d.y = 0;

        float distanceSqr = (target2d - position2d).sqrMagnitude;
        
        return distanceSqr < rangeSqr;
    }

    bool isDead(GameObject other)
    {
        return (other == null || other.GetComponent<Death>().Dead);
    }
}