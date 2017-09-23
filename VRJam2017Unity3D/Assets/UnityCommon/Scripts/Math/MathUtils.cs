namespace AltSrc.UnityCommon.Math
{
    public static class MathUtils
    {
        // https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
        // NOTE: If integer v is already a power of 2, it won't increase. -Casper 2017-09-14
        public static int RoundUpToNextPow2(int v)
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;

            return v;
        }
    }
}
