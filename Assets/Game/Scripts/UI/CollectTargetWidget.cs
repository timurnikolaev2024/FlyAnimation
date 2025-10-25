using UnityEngine;
using DG.Tweening;
using Game.Scripts;

namespace Game.Scripts
{
    public class CollectTargetWidget : MonoBehaviour, ICollectTarget
    {
        [SerializeField] private RectTransform _icon;
        [SerializeField] private EffectSettings _settings;

        private float _currentBoost;
        private Tween _decayTween;

        public RectTransform TargetRect => _icon;

        public void Pulse()
        {
            _currentBoost += _settings.pulseAdd;
            _currentBoost = Mathf.Min(_currentBoost, _settings.pulseMax - 1f);

            _icon.DOKill();
            _icon.localScale = Vector3.one * (1f + _currentBoost);

            _decayTween?.Kill();
            _decayTween = DOTween.To(
                () => _currentBoost,
                x =>
                {
                    _currentBoost = x;
                    _icon.localScale = Vector3.one * (1f + _currentBoost);
                },
                0f,
                _settings.pulseDecayTime
            ).SetEase(Ease.OutCubic);
        }
    }
}