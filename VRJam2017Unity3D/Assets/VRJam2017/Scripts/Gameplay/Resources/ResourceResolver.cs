using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ObjectDefinition))]
public class ResourceResolver : MonoBehaviour
{
    ResourceValue gas;

    ResourceValue minerals;

    ResourceValue supplies;

    ObjectDefinition definition;

    void Start()
    {
        ResourceValue[] resources = FindObjectsOfType<ResourceValue>();

        gas = Array.Find(resources, resource => resource.Type == ResourceType.Gas);

        minerals = Array.Find(resources, resource => resource.Type == ResourceType.Minerals);

        supplies = Array.Find(resources, resource => resource.Type == ResourceType.Supplies);

        definition = GetComponent<ObjectDefinition>();

        supplies.AddCapacity(definition.SupplyCapacity);
    }

    public bool CanAfford(ObjectDefinition definition)
    {
        return (minerals.CanSpend(definition.CostMinerals) && gas.CanSpend(definition.CostGas) && supplies.CanGather(definition.CostSupplies));
    }

    public bool Spend(ObjectDefinition definition)
    {
        return (minerals.Spend(definition.CostMinerals) && gas.Spend(definition.CostGas) && supplies.Gather(definition.CostSupplies));
    }

    public void Gather(ResourceType type, float amount)
    {
        ResourceValue resource = resolveType(type);

        if(resource.CanGather(amount))
        {
            resource.Gather(amount);
        }
    }

    ResourceValue resolveType(ResourceType type)
    {
        switch(type)
        {
            case ResourceType.Gas:
                return gas;
            case ResourceType.Minerals:
                return minerals;
            case ResourceType.Supplies:
                return supplies;
            default:
                throw new Exception("Resource type not defined");
        }
    }

}