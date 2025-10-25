using System;

namespace Game.Scripts
{
    public interface IEffect
    {
        void Play(in EffectContext ctx, Action onOneCollected = null);
    }
}