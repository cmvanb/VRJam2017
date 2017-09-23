using UnityEngine;
using UnityEditor;

namespace AltSrc.UnityCommon.Mono
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : Editor {}
}
