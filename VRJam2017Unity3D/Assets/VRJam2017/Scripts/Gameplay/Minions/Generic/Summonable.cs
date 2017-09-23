using UnityEngine;
using System.Collections;

public class Summonable : MonoBehaviour
{
    [SerializeField]
    float range;

    void Summon(GameObject target)
    {
        if(isInRange(target))
        {
            GetComponent<ActionQueue>().Set(new GameObjectMovementAction(gameObject, target));
        }
    }


    bool isInRange(GameObject other)
    {
        float rangeSqr = range * range;
        float distanceSqr = (other.transform.position - transform.position).sqrMagnitude;
        return distanceSqr < rangeSqr;
    }
}