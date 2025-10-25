using UnityEngine;

namespace Game.Scripts
{
    public interface ICollectTarget
    {
        RectTransform TargetRect { get; }
        void Pulse();
    }
}