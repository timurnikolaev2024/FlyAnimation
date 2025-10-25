using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Game.Scripts
{
    public class RiseCloudEffect : BaseCollectibleEffect
    {
        protected override Sequence BuildSequenceForOne(EffectContext ctx, Image img, Action onOneCollected)
        {
            var s = ctx.Settings;
            var start = ctx.StartPosWS;
            var targetPos = ctx.TargetPosWS;
            var targetRef = ctx.Target;

            Vector3 offset = new(
                UnityEngine.Random.Range(-s.spread.x, s.spread.x),
                UnityEngine.Random.Range(s.riseHeight * 0.7f, s.riseHeight * 1.2f),
                0f);

            float delay = UnityEngine.Random.Range(0f, 0.15f);

            var seq = DOTween.Sequence().SetAutoKill(true);
            seq.AppendInterval(delay);
            seq.Append(img.transform.DOMove(start + offset, s.riseDuration).SetEase(Ease.OutBack));
            seq.AppendInterval(s.holdDuration);
            seq.Append(img.transform.DOMove(targetPos, s.flyDuration).SetEase(Ease.InCubic))
                .OnComplete(() => Arrived(targetRef, img, onOneCollected));

            return seq;
        }
    }
}