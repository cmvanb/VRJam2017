using UnityEngine;

namespace AltSrc.UnityCommon.Math
{
    public static class Vector3Extensions
    {
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Vector3 Inverse(this Vector3 v)
        {
            return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
        }

        public static Vector3 MultiplyByScalar(this Vector3 v, float scalar)
        {
            return new Vector3(v.x * scalar, v.y * scalar, v.z * scalar);
        }

        public static Vector2 ToVec2XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 ToVec2XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
    }
}