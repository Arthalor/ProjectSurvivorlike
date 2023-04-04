using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class VectorFunctions
    {
        public static float SqrDistance(Vector3 a, Vector3 b) 
        {
            return Vector3.SqrMagnitude(a - b);
        }

        public static bool SqrDistanceSmaller(Vector3 a, Vector3 b, float compareDistance) 
        {
            return SqrDistance(a, b) < Mathf.Pow(compareDistance, 2);
        }

        public static bool SqrDistanceSmaller(Vector2 a, Vector2 b, float compareDistance) 
        {
            return SqrDistance(a, b) < Mathf.Pow(compareDistance, 2);
        }

        public static Vector2 VectorFromAtoB(Vector2 a, Vector2 b) 
        {
            return b - a;
        }

        public static Vector2 DirectionAtoB(Vector2 a, Vector2 b) 
        {
            return (b - a).normalized;
        }

        public static Vector2 Vector2NoZ(Vector3 input) 
        {
            return new Vector2(input.x, input.y);
        }
    }
}