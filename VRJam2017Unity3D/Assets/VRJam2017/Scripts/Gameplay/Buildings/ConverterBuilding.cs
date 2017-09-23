using UnityEngine;
using System.Collections;

public class ConverterBuilding : MonoBehaviour
{
    public void Release(GameObject toConvert)
    {
        toConvert.SendMessage("Convert", gameObject.GetComponent<Faction>().CurrentFaction);
    }
}