using System;
using UnityEngine;

namespace Helper
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float cooldownTime;
        private float cooldownClock;

        public Cooldown(float time) 
        {
            cooldownTime = time;
        }

        public void TickClock(float deltaTime) 
        {
            cooldownClock -= deltaTime;
        }

        public bool IsUp() 
        {
            if (cooldownClock < 0) return true;
            else return false;
        }

        public bool StartCooldown()
        {
            if (cooldownClock >= 0) return false;
            else
            {
                cooldownClock = cooldownTime;
                return true;
            }
        }
    }
}