using UnityEngine;
using System.Collections;

public class Healer : MonoBehaviour
{
    public float HealRange;

    public float HealPower;

    public float HealRate;

    GameObject currentlyHealing;
    
    void VisibleObjectSpotted(GameObject visibleObject)
    {
        if(currentlyHealing == visibleObject)
        {
            return;
        }

        if(currentlyHealing != null)
        {
            return;
        }

        ActionQueue queue = GetComponent<ActionQueue>();


        bool sameFaction = gameObject.GetComponent<Faction>().CurrentFaction == visibleObject.GetComponent<Faction>().CurrentFaction;

        if(sameFaction)
        {
            currentlyHealing = visibleObject;

            queue.InsertBeforeCurrent(new HealAction(gameObject, visibleObject));
        }
        
    }
    
}