using System;
using DG.Tweening;
using Game.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class GravityEffect : BaseCollectibleEffect
    {
        protected override Sequence BuildSequenceForOne(EffectContext ctx, Image img, Action onOneCollected)
        {
            var s = ctx.Settings;
            var startPos = ctx.StartPosWS;
            var targetPos = ctx.TargetPosWS;
            var targetRef = ctx.Target;

            Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 explosion = startPos +
                                (Vector3)(dir * UnityEngine.Random.Range(s.explosionPower * 0.5f, s.explosionPower));
            Vector3 fall = explosion + Vector3.down * s.fallDistance;

            var seq = DOTween.Sequence().SetAutoKill(true);
            seq.Append(img.transform.DOMove(explosion, 0.25f).SetEase(Ease.OutCubic));
            seq.Append(img.transform.DOMove(fall, s.gravityDuration).SetEase(Ease.InQuad));
            seq.Append(img.transform
                    .DOMove(targetPos, s.flyDuration)
                    .SetEase(Ease.OutCubic)
                    .OnStart(() => img.transform.DOScale(1.25f, 0.1f).SetLoops(2, LoopType.Yoyo)))
                .OnComplete(() => Arrived(targetRef, img, onOneCollected));

            return seq;
        }
    }
}