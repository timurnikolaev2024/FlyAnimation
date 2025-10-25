using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "FX/Collectible/EffectSettings", fileName = "DiamondEffectSettings")]
    public class EffectSettings : ScriptableObject
    {
        [Header("Common")]
        public int count = 8;

        [Header("Rise Cloud")]
        public float riseHeight = 120f;
        public Vector2 spread = new(80, 60);
        public float riseDuration = 0.35f;
        public float holdDuration = 0.25f;
        public float flyDuration = 0.7f;

        [Header("Spiral")]
        public float spiralRadius = 100f;
        public float spiralDuration = 0.7f;

        [Header("Gravity")]
        public float explosionPower = 200f;
        public float fallDistance = 120f;
        public float gravityDuration = 0.5f;

        [Header("Icon Pulse")]
        public float pulseAdd = 0.15f;
        public float pulseMax = 1.4f;
        public float pulseDecayTime = 0.6f;
    }
}