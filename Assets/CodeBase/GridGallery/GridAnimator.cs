using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.GridGallery
{
    public class GridAnimator : MonoBehaviour
    {
        public void PlayChangeFilterAnimation(ICollection<Transform> gridElements)
        {
            foreach (Transform element in gridElements)
            {
                if (element.gameObject.activeSelf)
                {
                    element.DOScale(element.localScale, 0.3f)
                        .From(element.localScale * 1.1f).SetEase(Ease.OutBack);
                }
            }
        }
    }
}