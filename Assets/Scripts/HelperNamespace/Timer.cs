using System;
using UnityEngine;

namespace Helper
{
    [Serializable]
    public class Timer
    {
        [SerializeField] private float maxTime;

        private float timer = 0;

        public Timer(float _maxTime) 
        {
            maxTime = _maxTime;
        }

        public Timer(float _maxTime, float initializedTime) 
        {
            maxTime = _maxTime;
            timer = initializedTime;
        }

        public void Reset() 
        {
            timer = 0;
        }

        public void Tick(float deltaTime) 
        {
            timer += deltaTime;
            if (timer > maxTime) timer = maxTime;
        }

        public float GetTime() 
        {
            return timer;
        }

        public float Progress() 
        {
            return timer / maxTime;
        }

        public bool Finished() 
        {
            return Progress() >= 1;
        }

        public float GetMaxTime() 
        {
            return maxTime;
        }
    }
}
