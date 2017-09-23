using UnityEngine;
using System.Collections;

public abstract class UnitAction
{
    public GameObject Owner;

    public UnitAction(GameObject owner)
    {
        Owner = owner;
    }

    public abstract IEnumerator Perform();

    public virtual void Place() {}

    public virtual void Stop() {}
}