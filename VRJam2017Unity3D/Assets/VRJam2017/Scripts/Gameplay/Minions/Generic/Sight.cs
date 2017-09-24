using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sight : MonoBehaviour
{
    [SerializeField]
    float range;

    void Start()
    {
        StartCoroutine(PerformCheck());
    }

    void FindNearbyObjects()
    {
        List<Visibility> objects = VisibilityRegister.Instance.AllObjects;

        foreach(Visibility visibilityObject in objects)
        {
            if(visibilityObject == GetComponent<Visibility>())
            {
                continue;                
            }
            if(visibilityObject.CurrentState == Visibility.State.Visible && isInRange(visibilityObject))
            {
                SendMessage("VisibleObjectSpotted", visibilityObject.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    IEnumerator PerformCheck()
    {
        while(true)
        {
            FindNearbyObjects();
            yield return new WaitForSeconds(1.0f);
        }
    }


    bool isInRange(Visibility other)
    {
        float rangeSqr = range * range;
        float distanceSqr = (other.transform.position - transform.position).sqrMagnitude;
        return distanceSqr < rangeSqr;
    }
}