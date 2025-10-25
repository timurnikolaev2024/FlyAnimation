using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using DG.Tweening;
using Game.Scripts;

namespace Game.Scripts
{
    public abstract class BaseCollectibleEffect : MonoBehaviour, IEffect
    {
        private ObjectPool<Image> _pool;

        protected virtual void Awake()
        {
            _pool = new ObjectPool<Image>(
                CreateImage,
                OnGet,
                OnRelease,
                OnDestroyImage,
                collectionCheck: false,
                defaultCapacity: 16,
                maxSize: 256
            );
        }

        public void Play(in EffectContext ctx, Action onOneCollected = null)
        {
            int count = Mathf.Max(0, ctx.Settings.count);
            float localTimeScale = Mathf.Max(0.01f, ctx.SpeedMultiplier);

            for (int i = 0; i < count; i++)
            {
                var img = _pool.Get();

                var prefab = ctx.CollectiblePrefab;
                if (prefab != null)
                {
                    img.sprite = prefab.sprite;
                    img.type = prefab.type;
                    img.preserveAspect = prefab.preserveAspect;
                    img.rectTransform.sizeDelta = prefab.rectTransform.sizeDelta;
                    img.color = prefab.color;
                }

                var rt = img.rectTransform;
                rt.SetParent(ctx.SpawnRoot, worldPositionStays: false);
                img.transform.position = ctx.StartPosWS;
                img.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.9f, 1.2f);
                img.raycastTarget = false;

                Sequence seq = BuildSequenceForOne(ctx, img, onOneCollected);

                if (seq != null && seq.active)
                {
                    seq.timeScale = localTimeScale;
                    ctx.RegisterTween?.Invoke(seq);
                }
                else
                {
                    _pool.Release(img);
                }
            }
        }

        protected void Arrived(ICollectTarget target, Image img, Action onOneCollected)
        {
            onOneCollected?.Invoke();
            target?.Pulse();

            img.transform
                .DOScale(1.3f, 0.1f)
                .OnComplete(() => _pool.Release(img));
        }

        protected abstract Sequence BuildSequenceForOne(EffectContext ctx, Image img, Action onOneCollected);

        private Image CreateImage()
        {
            var go = new GameObject("CollectibleFX",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image));

            var img = go.GetComponent<Image>();
            img.raycastTarget = false;
            return img;
        }

        private void OnGet(Image img)
        {
            if (img != null)
                img.gameObject.SetActive(true);
        }

        private void OnRelease(Image img)
        {
            if (img == null)
                return;

            var t = img.transform as RectTransform;
            if (t != null)
            {
                t.localScale = Vector3.one;
                t.localRotation = Quaternion.identity;
                t.anchoredPosition3D = Vector3.zero;
            }

            img.gameObject.SetActive(false);
            img.transform.SetParent(transform, worldPositionStays: false);
        }

        private void OnDestroyImage(Image img)
        {
            if (img != null)
                Destroy(img.gameObject);
        }
    }
}