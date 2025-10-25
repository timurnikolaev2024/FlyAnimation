using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class CounterWidget : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Color _gainColor = Color.green;
        [SerializeField] private float _pulseScale = 1.3f;
        [SerializeField] private float _pulseDur = 0.2f;

        private Color _baseColor;
        private CollectibleCounter _model;

        private void Awake() => _baseColor = _text.color;

        public void Bind(CollectibleCounter model)
        {
            _model = model;
            _model.Changed += OnChanged;
            OnChanged(_model.Value);
        }

        private void OnDestroy()
        {
            if (_model != null) _model.Changed -= OnChanged;
        }

        private void OnChanged(int val)
        {
            _text.text = val.ToString();

            _text.transform.DOKill();
            _text.DOKill();
            var s = DOTween.Sequence();
            s.Append(_text.transform.DOScale(_pulseScale, _pulseDur * 0.6f).SetEase(Ease.OutQuad));
            s.Join(_text.DOColor(_gainColor, _pulseDur * 0.6f));
            s.Append(_text.transform.DOScale(1f, _pulseDur).SetEase(Ease.InOutQuad));
            s.Join(_text.DOColor(_baseColor, _pulseDur));
        }
    }
}