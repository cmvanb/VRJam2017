using UnityEngine;

public class ResourceValue : MonoBehaviour
{
    public ResourceType Type;

    public float CurrentValue;

    public float Capacity = Mathf.Infinity;


    void Start()
    {
        SendMessage("ResourceCapacityChanged", this);
    }

    public bool CanSpend(float amount)
    {
        return amount <= CurrentValue;
    }

    public bool Spend(float amount)
    {
        if(CanSpend(amount))
        {
            CurrentValue -= amount;

            SendMessage("ResourceSpent", this);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanGather(float amount)
    {
        return (CurrentValue + amount <= Capacity);
    }

    public bool Gather(float amount)
    {
        if(CanGather(amount))
        {
            CurrentValue += amount;

            SendMessage("ResourceGathered", this);
        
            return true;
        }

        return false;        
    }

    public void AddCapacity(float amount)
    {
        Capacity += amount;

        SendMessage("ResourceCapacityChanged", this);
    }
}