using UnityEngine;
using System.Collections;

public class Capture : MonoBehaviour
{
    public float CaptureRange;

    GameObject currentlyCapturing;
    
    
    void VisibleObjectSpotted(GameObject visibleObject)
    {
        if(currentlyCapturing == visibleObject)
        {
            return;
        }

        if(currentlyCapturing != null)
        {
            return;
        }

        ActionQueue queue = GetComponent<ActionQueue>();

        bool capturable = gameObject.GetComponent<Faction>().IsCapturable(visibleObject);

        if(capturable && queue.IsCurrentInteruptable())
        {
            currentlyCapturing = visibleObject;

            ConverterBuilding converter = FindObjectOfType<ConverterBuilding>();

            queue.InsertBeforeCurrent(new CaptureAction(gameObject, converter.gameObject, visibleObject));
        }
        
    }
    
}