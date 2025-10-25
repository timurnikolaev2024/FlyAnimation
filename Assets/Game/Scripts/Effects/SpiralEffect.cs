using System;
using DG.Tweening;
using Game.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class SpiralEffect : BaseCollectibleEffect
    {
        protected override Sequence BuildSequenceForOne(EffectContext ctx, Image img, Action onOneCollected)
        {
            var s = ctx.Settings;
            var startPos = ctx.StartPosWS;
            var targetPos = ctx.TargetPosWS;
            var targetRef = ctx.Target;

            float angle = UnityEngine.Random.Range(0f, 360f);
            float delay = UnityEngine.Random.Range(0f, 0.15f);
            float radius = UnityEngine.Random.Range(s.spiralRadius * 0.8f, s.spiralRadius * 1.2f);

            Vector3 spiralOffset = new(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius + 50f,
                0f);

            var seq = DOTween.Sequence().SetAutoKill(true);
            seq.AppendInterval(delay);
            seq.Append(img.transform
                .DOBlendableLocalRotateBy(new Vector3(0, 0, 360f), s.spiralDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear));
            seq.Join(img.transform
                .DOMove(startPos + spiralOffset, s.spiralDuration)
                .SetEase(Ease.OutBack));
            seq.Append(img.transform
                    .DOMove(targetPos, s.flyDuration)
                    .SetEase(Ease.InCubic))
                .OnComplete(() => Arrived(targetRef, img, onOneCollected));

            return seq;
        }
    }
}