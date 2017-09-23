using UnityEngine;
using System.Collections;

public class Healer : MonoBehaviour
{
    public float HealRange;

    public float HealPower;

    public float HealRate;
    
    void VisibleObjectSpotted(GameObject visibleObject)
    {
        ActionQueue queue = GetComponent<ActionQueue>();


        bool sameFaction = gameObject.GetComponent<Faction>().CurrentFaction == visibleObject.GetComponent<Faction>().CurrentFaction;

        if(sameFaction)
        {
            queue.InsertBeforeCurrent(new HealAction(gameObject, visibleObject));
        }
        
    }
    
}