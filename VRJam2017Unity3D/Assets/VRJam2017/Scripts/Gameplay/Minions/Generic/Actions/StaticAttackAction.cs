
using UnityEngine;
using System.Collections;

public class StaticAttackAction : GameObjectAction
{
    public StaticAttackAction(GameObject owner, GameObject gameObject) : base(owner, gameObject)
    {
    }

    public override IEnumerator Perform()
    {
        while(!isDead(GameObject) && isInRange(GameObject))
        {
            Attack attack = Owner.GetComponent<Attack>();

            GameObject.SendMessage("WantsToDamage", attack.AttackPower);

            yield return new WaitForSeconds(attack.FireRate);
        }

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