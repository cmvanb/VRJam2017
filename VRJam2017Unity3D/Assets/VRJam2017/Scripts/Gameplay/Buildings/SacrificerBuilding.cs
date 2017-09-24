using UnityEngine;
using System.Collections;

public class SacrificerBuilding : MonoBehaviour
{
    public void Release(GameObject toConvert)
    {
        toConvert.SendMessage("Sacrifice", gameObject.GetComponent<Faction>().CurrentFaction);
    }
}