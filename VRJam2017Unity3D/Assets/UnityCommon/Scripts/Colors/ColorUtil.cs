// NOTE: Original from https://github.com/lordofduct/spacepuppy-unity-framework

using UnityEngine;

namespace AltSrc.UnityCommon.Colors
{
    public static class ColorUtil
    {
        public static int ToInt(Color color)
        {
            return
                (Mathf.RoundToInt(color.a * 255) << 24) +
                (Mathf.RoundToInt(color.r * 255) << 16) +
                (Mathf.RoundToInt(color.g * 255) << 8) +
                Mathf.RoundToInt(color.b * 255);
        }

        public static Color ToColor(int value)
        {
            var a = (float)(value >> 24 & 0xFF) / 255f;
            var r = (float)(value >> 16 & 0xFF) / 255f;
            var g = (float)(value >> 8 & 0xFF) / 255f;
            var b = (float)(value & 0xFF) / 255f;

            return new Color(r, g, b, a);
        }

        public static Color ToColor(Vector3 value)
        {
            return new Color(
                (float)value.x,
                (float)value.y,
                (float)value.z);
        }

        public static Color ToColor(Vector4 value)
        {
            return new Color(
                (float)value.x,
                (float)value.y,
                (float)value.z,
                (float)value.w);
        }
    }
}
