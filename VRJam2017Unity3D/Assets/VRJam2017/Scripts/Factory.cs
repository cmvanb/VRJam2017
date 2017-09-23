using UnityEngine;
using System;

[System.Serializable]
public class Factory : MonoBehaviour
{
    public GameObject build(GameObject prefab, Vector3 position)
    {
        //ObjectDefinition prefab = Array.Find(objects, item => item.Name == name);
        return Instantiate(prefab, position, Quaternion.identity);
    }
}
