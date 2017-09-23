using UnityEngine;

namespace AltSrc.UnityCommon.Math
{
    public struct LineSegment2D
    {
        public Vector2 PointA { get; set; }
        public Vector2 PointB { get; set; }

        // TODO: Consider optimizing this calculation by caching it. -Casper 2017-08-17
        public Rect Bounds
        {
            get
            {
                float x = Mathf.Min(PointA.x, PointB.x);
                float y = Mathf.Min(PointA.y, PointB.y);
                float width = Mathf.Abs(PointA.x - PointB.x);
                float height = Mathf.Abs(PointA.y - PointB.y);

                return new Rect(x, y, width, height);
            }
        }

        // TODO: Consider optimizing this calculation by caching it. -Casper 2017-08-31
        public float DirectionInDegrees
        {
            get
            {
                //return Vector2.Angle(PointA, PointB);
                return 180f - (Mathf.Atan2(PointB.y - PointA.y, PointB.x - PointA.x) * Mathf.Rad2Deg);
            }
        }

        public float Length
        {
            get
            {
                return this.ToVector2().magnitude;
            }
        }

        public Vector2 MidPoint
        {
            get
            {
                return (PointA + PointB) / 2f;
            }
        }

        public LineSegment2D(Vector2 pointA, Vector2 pointB)
        {
            this.PointA = pointA;
            this.PointB = pointB;
        }

        public Vector2 ToVector2()
        {
            return PointB - PointA;
        }

        /// <summary>
        ///   Check whether two finite line segments intersect.
        ///   NOTE: The out values may not always exist.
        ///       @param segmentA - Finite line segment.
        ///       @param segmentB - Finite line segment.
        ///       @out i0         - Intersection point (if it exists)
        ///       @out i1         - Intersection end point (if the segments overlap)
        ///       @returns        - 0: Disjointed, no intersection.
        ///                         1: Intersection at unique point 'intersectPoint'.
        ///                         2: Overlapping segments from 'intersectPoint' to 'intersectEndpoint'.
        /// </summary>
        public static int CheckIntersection(
            LineSegment2D segmentA,
            LineSegment2D segmentB,
            out Vector2 i0,
            out Vector2 i1)
        {
            Vector2 u = segmentA.PointB - segmentA.PointA;
            Vector2 v = segmentB.PointB - segmentB.PointA;
            Vector2 w = segmentA.PointA - segmentB.PointA;
            float D = ArePerpindicular(u, v);

            i0 = Vector2.zero;
            i1 = Vector2.zero;

            // test if  they are parallel (includes either being a point)
            if (Mathf.Abs(D) < 0.01f)
            {
                // segmentA and segmentB are parallel

                // test if they are NOT collinear
                if (ArePerpindicular(u, w) != 0 || ArePerpindicular(v, w) != 0)
                {
                    return 0;
                }

                // they are collinear or degenerate
                // check if they are degenerate points
                float du = Vector2.Dot(u, u);
                float dv = Vector2.Dot(v, v);

                // both segments are points
                if (du == 0 && dv == 0)
                {
                    // they are distinct points
                    if (segmentA.PointA != segmentB.PointA)
                    {
                        return 0;
                    }

                    // they are the same point
                    i0 = segmentA.PointA;
                    return 1;
                }

                // test if segmentA is a single point
                if (du == 0)
                {
                    // but is not in segmentB
                    if (ContainsPoint(segmentA.PointA, segmentB) == 0)
                    {
                        return 0;
                    }

                    i0 = segmentA.PointA;
                    return 1;
                }

                // test if segmentB is a single point
                if (dv == 0)
                {
                    // but is not in segmentA
                    if (ContainsPoint(segmentB.PointA, segmentA) == 0)
                    {
                        return 0;
                    }

                    i0 = segmentB.PointA;
                    return 1;
                }

                // they are collinear segments - test for overlap (or not)

                // endpoints of segmentA in eqn for segmentB
                float t0, t1;
                Vector2 w2 = segmentA.PointB - segmentB.PointA;

                if (v.x != 0)
                {
                    t0 = w.x / v.x;
                    t1 = w2.x / v.x;
                }
                else
                {
                    t0 = w.y / v.y;
                    t1 = w2.y / v.y;
                }

                // must have t0 smaller than t1

                // swap if not
                if (t0 > t1)
                {
                    float t = t0;
                    t0 = t1;
                    t1 = t;
                }

                if (t0 > 1 || t1 < 0)
                {
                    // NO overlap
                    return 0;
                }

                t0 = t0 < 0 ? 0 : t0;               // clip to min 0
                t1 = t1 > 1 ? 1 : t1;               // clip to max 1

                if (t0 == t1)
                {
                    // intersect is a point
                    i0 = segmentB.PointA + t0 * v;
                    return 1;
                }

                // they overlap in a valid subsegment
                i0 = segmentB.PointA + t0 * v;
                i1 = segmentB.PointA + t1 * v;

                return 2;
            }

            // the segments are skew and may intersect in a point
            // get the intersect parameter for segmentA
            float sI = ArePerpindicular(v, w) / D;

            // no intersect with segmentA
            if (sI < 0 || sI > 1)
            {
                return 0;
            }

            // get the intersect parameter for segmentB
            float tI = ArePerpindicular(u, w) / D;

            // no intersect with segmentB
            if (tI < 0 || tI > 1)
            {
                return 0;
            }

            // compute segmentA intersect point
            i0 = segmentA.PointA + sI * u;

            return 1;
        }

        /// <summary>
        ///   Determine if a point is inside a segment
        ///       @param P - point
        ///       @param S - collinear segment
        ///       @returns - 0 = P is  not inside S
        ///                  1 = P is inside S
        /// </summary>
        public static int ContainsPoint(Vector2 P, LineSegment2D S)
        {
            // S is not vertical
            if (S.PointA.x != S.PointB.x)
            {
                if ((S.PointA.x <= P.x && P.x <= S.PointB.x)
                    || (S.PointA.x >= P.x && P.x >= S.PointB.x))
                {
                    return 1;
                }
            }
            // S is vertical, so test y coordinate
            else
            {
                if ((S.PointA.y <= P.y && P.y <= S.PointB.y)
                    || (S.PointA.y >= P.y && P.y >= S.PointB.y))
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        ///   Calculate the smallest difference in angles between two line segments, in degrees.
        ///       @param a - line segment
        ///       @param b - line segment
        ///       @returns - the smallest angle between the line segments
        /// </summary>
        public static float MinimumAngleDifferenceInDegrees(LineSegment2D a, LineSegment2D b)
        {
            float diff = Mathf.Abs(a.DirectionInDegrees - b.DirectionInDegrees) % 180f;

            return Mathf.Min(diff, Mathf.Abs(diff - 180f));
        }

        /// <summary>
        ///   Calculate whether two vectors are perpindicular or not.
        ///       @param u - point
        ///       @param v - point
        ///       @returns - true if u and v are perpindicular, otherwise false
        /// </summary>
        public static float ArePerpindicular(Vector2 u, Vector2 v)
        {
            return ((u).x * (v).y - (u).y * (v).x);
        }

        /// <summary>
        ///   Find the distance from a point to a line segment. This is done by projecting the vector
        ///   from point to segment.PointA onto the line segment, then calculating the distance between
        ///   the projected point and original point.
        ///       @param segment - the line segment
        ///       @param point - the point
        ///       @out projectedPoint - vector point -> segment.PointA projected onto segment
        ///       @out projectedLineLength - the length of vector point -> segment.PointA
        ///       @returns the distance between point and projectedPoint
        /// </summary>
        public static float FindDistanceToPoint(
            LineSegment2D segment,
            Vector2 point,
            out Vector2 projectedPoint,
            out float projectedLineLength)
        {
            Vector2 segmentVector = segment.ToVector2();

            Vector2 projectingVector = point - segment.PointA;

            float dotProduct = Mathf.Infinity;

            Vector2 projectedVector = segmentVector.Project(projectingVector, out dotProduct);

            projectedPoint = segment.PointA + projectedVector;

            projectedLineLength = projectedVector.magnitude;

            return Vector2.Distance(projectedPoint, point);
        }

        public static float ShortestDistanceBetweenLineSegments(LineSegment2D s1, LineSegment2D s2)
        {
            System.Func<LineSegment2D, Vector2, float> validDistance = 
                (LineSegment2D s, Vector2 p) => 
                {
                    Vector2 projectedPoint = Vector2.zero;

                    float projectedLineLength = Mathf.Infinity;

                    float result = LineSegment2D.FindDistanceToPoint(
                        s, p, out projectedPoint, out projectedLineLength);

                    if (projectedLineLength >= 0f
                        && projectedLineLength <= s.Length)
                    {
                        return result;
                    }

                    return Mathf.Infinity;
                };

            float distanceS1A = validDistance(s2, s1.PointA);
            float distanceS1B = validDistance(s2, s1.PointB);
            float distanceS2A = validDistance(s1, s2.PointA);
            float distanceS2B = validDistance(s1, s2.PointB);

            return Mathf.Min(distanceS1A, distanceS1B, distanceS2A, distanceS2B);
        }
    }
}
