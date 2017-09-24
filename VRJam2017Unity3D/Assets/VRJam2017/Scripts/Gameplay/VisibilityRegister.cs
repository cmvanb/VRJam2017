using System.Collections;
using System.Collections.Generic;
using AltSrc.UnityCommon.Patterns;
using UnityEngine;

public class VisibilityRegister : MonoSingleton<VisibilityRegister>
{
	private List<Visibility> objects = new List<Visibility>();

    public List<Visibility> AllObjects
    {
        get { return objects; }
    }

	public void Register(Visibility visibiltyObject)
	{
		objects.Add(visibiltyObject);
	}

	public void Deregister(Visibility visibiltyObject)
	{
		objects.Remove(visibiltyObject);
	}
}
