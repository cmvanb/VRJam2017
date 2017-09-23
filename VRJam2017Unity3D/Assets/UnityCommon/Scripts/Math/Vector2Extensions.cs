using UnityEngine;

namespace AltSrc.UnityCommon.Math
{
    public static class Vector2Extensions
    {
        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        public static Vector2 Inverse(this Vector2 v)
        {
            return new Vector2(1f / v.x, 1f / v.y);
        }

        public static Vector2 Project(this Vector2 v, Vector2 p, out float dotProduct)
        {
            dotProduct = Vector2.Dot(p, v);

            return v.MultiplyByScalar(dotProduct / v.magnitude);
        }

        public static Vector2 MultiplyByScalar(this Vector2 v, float scalar)
        {
            return new Vector2(v.x * scalar, v.y * scalar);
        }

        public static Vector3 ToVec3XY(this Vector2 v)
        {
            return new Vector3(v.x, v.y, 0f);
        }

        public static Vector3 ToVec3XZ(this Vector2 v)
        {
            return new Vector3(v.x, 0f, v.y);
        }
    }
}

