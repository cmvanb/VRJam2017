using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerGrabber : MonoBehaviour 
{
    public Transform RightHand;

    public float GrabRange = 1f;

    private GameObject grabbedMinion = null;

    public bool IsGrabbing
    {
        get
        {
            return grabbedMinion != null;
        }
    }

    public void Grab()
    {
        GameObject minionInGrabRange = GetMinionInGrabRange();

        if (minionInGrabRange != null)
        {
            minionInGrabRange.SendMessage("Capture");
            minionInGrabRange.transform.parent = RightHand;

            grabbedMinion = minionInGrabRange;
        }
    }

    public void Drop()
    {
        if (grabbedMinion != null)
        {
            grabbedMinion.SendMessage("Release");
            grabbedMinion.transform.parent = null;

            grabbedMinion = null;
        }
    }

    private GameObject GetMinionInGrabRange()
    {
        GameObject nearest = MinionManager.Instance.GetNearestMinion(RightHand.gameObject);

        float distance = Vector3.Distance(nearest.transform.position, RightHand.position);

        if (nearest != null
            && distance < GrabRange)
        {
            return nearest;
        }

        return null;
    }
}
