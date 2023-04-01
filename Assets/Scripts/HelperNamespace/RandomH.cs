using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class RandomH
    {
        public static T RandomFromList<T>(List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T RandomFromArray<T>(T[] array) 
        {
            return array[Random.Range(0, array.Length)];
        }

        public static bool Coinflip() 
        {
            return Random.Range(0, 2) > 0;
        }
    }
}