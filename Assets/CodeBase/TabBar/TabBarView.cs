using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.TabBar
{
    public class TabBarView : MonoBehaviour
    {
        public event Action SelectedFilterChanged;

        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        
        [SerializeField] private TabElement[] _tabElements;
        [SerializeField] private Transform _selectIndicator;

        public TabFilterType SelectedFilter { get; private set; } = TabFilterType.Odd;

        private void OnEnable()
        {
            foreach (TabElement tabElement in _tabElements) 
                tabElement.Button.onClick.AddListener(() => OnClickButton(tabElement.FilterType));
        }

        private void OnDisable()
        {
            foreach (TabElement tabElement in _tabElements) 
                tabElement.Button.onClick.RemoveAllListeners();
        }

        private void OnClickButton(TabFilterType filterType)
        {
            foreach (TabElement tabElement in _tabElements)
            {
                if (tabElement.FilterType == filterType)
                {
                    PlayClickAnimation(tabElement.Button.transform);
                    tabElement.Text.color = _selectedColor;
                    _selectIndicator.DOMoveX(tabElement.Button.transform.position.x, 0.2f);
                }
                else
                {
                    tabElement.Text.color = _unselectedColor;
                }
            }

            if (filterType != SelectedFilter)
            {
                SelectedFilter = filterType;
                SelectedFilterChanged?.Invoke();
            }
        }

        private void PlayClickAnimation(Transform targetTransform)
        {
            DOTween.Kill(transform);
            targetTransform.localScale = Vector3.one;
            
            DOTween.Sequence().SetId(transform)
                .Append(targetTransform.DOScaleX(1.3f, 0.2f)).SetId(transform)
                .Append(targetTransform.DOScaleX(1, 0.2f)).SetEase(Ease.OutBounce).SetId(transform);
        }
    }
}