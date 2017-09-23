using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColor : MonoBehaviour {

	// Use this for initialization
	void Damage(float amount)
	{
		StartCoroutine(this.changeColor());
	}

	IEnumerator changeColor()
	{
		GetComponentInChildren<Renderer>().material.color = Color.red;

		yield return new WaitForSeconds(0.1f);

		GetComponentInChildren<Renderer>().material.color = Color.white;
	}
}
