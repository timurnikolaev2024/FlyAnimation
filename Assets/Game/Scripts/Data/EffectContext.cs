using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    [Serializable]
    public struct EffectContext
    {
        public RectTransform Start;
        public ICollectTarget Target;
        public RectTransform SpawnRoot;
        public Image CollectiblePrefab;
        public EffectSettings Settings;
        public float SpeedMultiplier;

        public Action<Tween> RegisterTween;
        public Vector3 StartPosWS => Start.position;
        public Vector3 TargetPosWS => Target.TargetRect.position;
    }
}