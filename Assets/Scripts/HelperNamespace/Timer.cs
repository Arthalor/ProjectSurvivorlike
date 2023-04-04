using System;
using UnityEngine;

namespace Helper
{
    public class Timer
    {
        private float maxTime;

        private float timer = 0;

        public Timer(float _maxTime) 
        {
            maxTime = _maxTime;
        }

        public void ResetTimer() 
        {
            timer = 0;
        }

        public void TickTimer(float deltaTime) 
        {
            timer += deltaTime;
            if (timer > maxTime) timer = maxTime;
        }

        public float TimerProgress() 
        {
            return timer / maxTime;
        }

        public bool TimerFinished() 
        {
            return TimerProgress() >= 1;
        }

        public float GetMaxTime() 
        {
            return maxTime;
        }
    }
}
