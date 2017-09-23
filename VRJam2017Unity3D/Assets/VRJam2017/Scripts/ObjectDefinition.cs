using System;
using UnityEngine;

[Serializable]
public class ObjectDefinition : MonoBehaviour
{
    public enum PlacementType
    {
        GotoWaypoint,
        RequiresPlacement
    }

    public string Name;
    
    public float CostMinerals;

    public float CostGas;

    public float CostSupplies;

    public float SupplyCapacity;

    public float BuildTime;

    public Sprite Icon;

    public PlacementType Placement;

    public ObjectDefinition[] Prerequisites;


}