using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePatch : MonoBehaviour 
{
    [SerializeField]
    float currentValue;

    public ResourceType Type; 


    public float Harvest(float amount)
    {
        if(currentValue < amount)
        {
            SendMessage("WantsToDie");

            return currentValue;
        }
        else
        {
            currentValue -= amount;

            return amount;
        }
    }

    public bool IsEmpty()
    {
        return currentValue == 0;
    }

    void WantsToDie()
    {
        currentValue = 0;
    }

}
