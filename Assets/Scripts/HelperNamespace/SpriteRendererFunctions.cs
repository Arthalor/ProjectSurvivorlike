using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class SpriteRendererFunctions
    {
        public static void FlipSpriteToMovement(SpriteRenderer renderer, float velocity)
        {
            //Boolean Expression derived from this theoretical If Expression:
            //if (Velocity X > 0) Flip False;
            //if (Velocity X < 0) Flip True;
            //if (Velocity X = 0) Flip = OldFlip
            bool a = velocity >= 0;
            bool b = velocity <= 0;
            bool c = renderer.flipX;
            renderer.flipX = (!a && b) || (!a && c) || (b && c);
        }
    }
}