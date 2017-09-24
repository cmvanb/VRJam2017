using UnityEngine;
using System.Collections;

public class Summonable : MonoBehaviour
{
    [SerializeField]
    float range;

    public bool BeingSummoned;

    void Summon(GameObject target)
    {
        if (isInRange(target))
        {
            GetComponent<ActionQueue>().Set(new SummonAction(gameObject, target));
        }
        else
        {
            Debug.LogWarning("OUT OF RANGE");
        }
    }


    bool isInRange(GameObject other)
    {
        float rangeSqr = range * range;
        float distanceSqr = (other.transform.position - transform.position).sqrMagnitude;
        return distanceSqr < rangeSqr;
    }
}