using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace Game.Scripts
{
    public class CollectibleButtonItem : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private Button _button;
        [SerializeField] private RectTransform _buttonRect;
        [SerializeField] private Slider _speedSlider;

        [Header("FX Bindings")] [SerializeField]
        private RectTransform _spawnRoot;

        [SerializeField] private Image _collectiblePrefab;
        [SerializeField] private EffectSettings _settings;
        [SerializeField] private MonoBehaviour _effectComponent; // IEffect
        [SerializeField] private MonoBehaviour _targetComponent; // ICollectTarget

        [Header("Counter")] [SerializeField] private CounterWidget _counterWidget;

        private readonly List<Tween> _activeTweens = new();
        private IEffect _effect;
        private ICollectTarget _target;
        private CollectibleCounter _counter;
        private float _speedMul = 1f;
        private Vector3 _baseEuler;
        private Vector3 _baseScale;
        private Sequence _btnSeq;

        private void Awake()
        {
            _effect = (IEffect)_effectComponent;
            _target = (ICollectTarget)_targetComponent;

            _counter = new CollectibleCounter();
            _counterWidget.Bind(_counter);

            _button.onClick.AddListener(OnClick);

            if (_speedSlider)
            {
                _speedSlider.minValue = 0.01f;
                _speedSlider.maxValue = 3f;
                _speedSlider.value = 1f;
                _speedSlider.onValueChanged.AddListener(OnSpeedChanged);
            }

            _baseEuler = _buttonRect.localEulerAngles;
            _baseScale = _buttonRect.localScale;
        }

        private void OnSpeedChanged(float v)
        {
            _speedMul = Mathf.Clamp(v, 0.01f, 3f);

            for (int i = _activeTweens.Count - 1; i >= 0; i--)
            {
                var t = _activeTweens[i];
                if (t == null || !t.active)
                {
                    _activeTweens.RemoveAt(i);
                    continue;
                }

                t.timeScale = _speedMul;
            }
        }

        private void OnClick()
        {
            var ctx = new EffectContext
            {
                Start = _buttonRect,
                Target = _target,
                SpawnRoot = _spawnRoot,
                CollectiblePrefab = _collectiblePrefab,
                Settings = _settings,
                SpeedMultiplier = _speedMul,
                RegisterTween = RegisterTween
            };

            _effect.Play(in ctx, OnOneCollected);
            PlayButtonPress();
        }

        private void RegisterTween(Tween t)
        {
            if (t == null) return;
            _activeTweens.Add(t);
            t.OnKill(() => _activeTweens.Remove(t));
            t.timeScale = _speedMul;
        }

        private void OnOneCollected() => _counter.Add(1);

        private void PlayButtonPress()
        {
            if (_btnSeq != null && _btnSeq.IsActive()) _btnSeq.Kill();
            _buttonRect.localEulerAngles = _baseEuler;
            _buttonRect.localScale = _baseScale;

            _button.interactable = false;

            _btnSeq = DOTween.Sequence()
                .SetAutoKill(false)
                .OnComplete(() =>
                {
                    _buttonRect.localEulerAngles = _baseEuler;
                    _buttonRect.localScale = _baseScale;
                    _button.interactable = true;
                });

            _btnSeq.Append(
                _buttonRect.DOLocalRotate(_baseEuler + new Vector3(10f, 0f, 0f), 0.08f).SetEase(Ease.OutQuad));
            _btnSeq.Join(_buttonRect.DOScaleY(_baseScale.y * 0.92f, 0.08f).SetEase(Ease.OutQuad));
            _btnSeq.AppendInterval(0.05f);
            _btnSeq.Append(_buttonRect.DOLocalRotate(_baseEuler, 0.08f).SetEase(Ease.OutBack));
            _btnSeq.Join(_buttonRect.DOScaleY(_baseScale.y, 0.08f).SetEase(Ease.OutBack));

            _btnSeq.Restart();
        }
    }
}