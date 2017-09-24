using System.Collections;
using System.Collections.Generic;
using AltSrc.UnityCommon.Patterns;
using UnityEngine;

public class MinionManager : MonoSingleton<MinionManager>
{
    public GameObject MinionPrefab;

	public Faction.FactionType FactionType = Faction.FactionType.Hell;

	private List<GameObject> minions = new List<GameObject>();

	private List<LevelTile> digList = new List<LevelTile>();

	void Start()
	{
		StartCoroutine(ProcessDigList());
	}

    public GameObject SpawnMinion(Vector3 position)
    {
        GameObject minion = GameObject.Instantiate(MinionPrefab, position, Quaternion.identity);

        return minion;
    }

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

	public void AddDigTile(LevelTile tile)
	{
		digList.Add(tile);
	}

	public void RemoveDigTile(LevelTile tile)
	{
		digList.Remove(tile);
	}

	void Update()
	{
		if(Input.GetMouseButton(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Vector2 tilePos = LevelHelpers.TilePosFromWorldPos(hit.point);

				LevelTile tile = LevelController.Instance.Model.Tiles[(int)tilePos.x, (int)tilePos.y];

				if(tile != null && !digList.Contains(tile))
				{
					AddDigTile(tile);
				}
			}
		}
	}

	IEnumerator ProcessDigList()
	{
		while(true)
		{
			digList.RemoveAll(tile => tile.Opened);

			digList.ForEach(tile => {
				// Do we have surrounding tiles in Hell?
				List<LevelTile> surrounding = LevelHelpers.GetSurroundingTiles(LevelController.Instance.Model, tile.X, tile.Z);
				LevelTile found = surrounding.Find(obj => (obj.Opened && LevelController.Instance.IsTileInHell(obj.X, obj.Z)));

				// If so, dig them with some minions
				if(found != null)
				{
					for(int i=0; i<5; i++)
					{
						GameObject minion = GetNearestNonBusyMinion(tile);

						if(minion)
						{
							minion.GetComponent<ActionQueue>().InsertBeforeCurrent(new DigAction(minion, new Vector2(tile.X, tile.Z)));
						}
					}
				}
			});

			yield return new WaitForSeconds(1);
		}
	}

	public GameObject GetNearestNonBusyMinion(LevelTile tile)
	{
		float closestDist = Mathf.Infinity;
		GameObject closest = null;

		foreach(GameObject minion in minions)
		{
			if(minion.GetComponent<ActionQueue>().IsBusy())
			{
				continue;
			}

			Vector3 pos = LevelHelpers.WorldPosFromTilePos(tile.X, tile.Z);

			float dist = (minion.transform.position - pos).sqrMagnitude;
			if(dist < closestDist)
			{
				closestDist = dist;
				
				closest = minion;
			}
		}

		return closest;
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
