using DG.Tweening;
using UnityEngine;

namespace CodeBase
{
    public static class TweenExtensions
    {
        public static void DoClickShake(this Transform transform, Vector3 startScale)
        {
            DOTween.Kill($"{transform}ClickShake");
            transform.localScale = startScale;
            
            DOTween.Sequence()
                .Append(transform.DOScale(transform.localScale * 0.9f, .3f))
                .SetId($"{transform}ClickShake")
                .Append(transform.DOScale(transform.localScale, .5f)).SetEase(Ease.OutBack)
                .SetId($"{transform}ClickShake");
        }
    }
}