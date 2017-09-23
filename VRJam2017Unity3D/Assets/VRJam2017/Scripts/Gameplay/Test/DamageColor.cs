using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColor : MonoBehaviour {

	[SerializeField]
	Color defaultColor;

	void Start()
	{
		GetComponentInChildren<Renderer>().material.color = defaultColor;
	}

	void Damage(float amount)
	{
		StartCoroutine(this.changeColor(Color.white));
	}

	void Heal(float amount)
	{
		StartCoroutine(this.changeColor(Color.green));
	}

	IEnumerator changeColor(Color color)
	{
		GetComponentInChildren<Renderer>().material.color = color;

		yield return new WaitForSeconds(0.01f);

		GetComponentInChildren<Renderer>().material.color = defaultColor;
	}
}
