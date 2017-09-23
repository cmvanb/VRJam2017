using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTester : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Vector3 pos = new Vector3(10,0,10);
		
		UnitAction action = new TerrainAction(gameObject, pos);

		GetComponent<ActionQueue>().Add(action);
		
		pos = new Vector3(-10,0,10);

		action = new TerrainAction(gameObject, pos);

		GetComponent<ActionQueue>().Add(action);

		pos = new Vector3(0,0,0);

		action = new TerrainAction(gameObject, pos);

		GetComponent<ActionQueue>().Add(action);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
