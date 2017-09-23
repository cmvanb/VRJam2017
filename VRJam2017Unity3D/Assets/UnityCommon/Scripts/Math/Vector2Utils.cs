using UnityEngine;

namespace AltSrc.UnityCommon.Math
{
    public static class Vector2Utils
    {
        public static Vector2 FromAngleInDegreesAndMagnitude(float angle, float magnitude)
        {
            angle = 180f - angle;
            angle = angle * Mathf.Deg2Rad;

            return new Vector2(magnitude * Mathf.Cos(angle), magnitude * Mathf.Sin(angle));
        }
    }
}

