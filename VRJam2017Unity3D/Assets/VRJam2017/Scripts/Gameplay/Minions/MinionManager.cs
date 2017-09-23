using System.Collections;
using System.Collections.Generic;
using AltSrc.UnityCommon.Patterns;
using UnityEngine;

public class MinionManager : MonoSingleton<MinionManager>
{
	public Faction.FactionType FactionType = Faction.FactionType.Hell;

	List<GameObject> minions = new List<GameObject>();

	public void Register(GameObject minion)
	{
		minions.Add(minion);

		Debug.Log("Minion registered: " + minion.name);
	}

	public void Deregister(GameObject minion)
	{
		minions.Remove(minion);
	}

	public GameObject SummonOne(GameObject source)
	{
		GameObject nearest = GetNearestMinion(source);

		if(nearest != null)
		{
			nearest.SendMessage("Summon", source);
		}

		return nearest;
	}

	public GameObject GetNearestMinion(GameObject source)
	{
		float closestDist = Mathf.Infinity;
		GameObject closest = null;

		foreach(GameObject minion in minions)
		{
			if(!minion.GetComponent<ActionQueue>().IsCurrentInteruptable())
			{
				continue;
			}

			float dist = (minion.transform.position - source.transform.position).sqrMagnitude;
			if(dist < closestDist)
			{
				closestDist = dist;
				
				closest = minion;
			}
		}

		return closest;
	}
}
