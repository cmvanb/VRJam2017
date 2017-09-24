using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHandType : MonoBehaviour {

	public enum HandTypes
	{
		Default,
		Grab,
		Point
	}

	public GameObject handDefault;
    public GameObject handGrab;
    public GameObject handPoint;

	public void SwitchHand(HandTypes newType)
	{
        switch (newType)
        {
            case HandTypes.Default:
                handDefault.SetActive(true);
                handGrab.SetActive(false);
                handPoint.SetActive(false);
                break;
            case HandTypes.Grab:
                handDefault.SetActive(false);
                handGrab.SetActive(true);
                handPoint.SetActive(false);
                break;
            case HandTypes.Point:
                handDefault.SetActive(false);
                handGrab.SetActive(false);
                handPoint.SetActive(true);
                break;
        }
    }

}
