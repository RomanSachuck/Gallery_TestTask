using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Banners
{
    public class Carousel : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private RectTransform[] _banners;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Toggles _toggles;
        [SerializeField] private AutoSwipe _autoSwipe;
        [SerializeField] private float _swipeThreshold = 1;
        [SerializeField] private float _swipeDuration = 0.4f;
    
        private float[] _positions;
        private float _distance;
        private int _currentIndex = 1;
        private float _startDragPosition;

        private void OnEnable() => 
            _autoSwipe.SwipeTime += OnSwipeTime;

        private void OnDisable() => 
            _autoSwipe.SwipeTime -= OnSwipeTime;

        void Start()
        {
            float canvasWidth = _canvasRect.rect.width;
            
            int bannersCount = _banners.Length;
            _positions = new float[bannersCount];
            
            _distance = 1f / (bannersCount - 1);
        
            for (int i = 0; i < bannersCount; i++)
            {
                _positions[i] = i * _distance;
                
                Vector2 sizeDelta = _banners[i].sizeDelta;
                sizeDelta.x = canvasWidth;
                _banners[i].sizeDelta = sizeDelta;
            }
            
            _scrollRect.horizontalNormalizedPosition = _positions[_currentIndex];
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            
            _startDragPosition = _scrollRect.horizontalNormalizedPosition;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            float swipe = _startDragPosition - _scrollRect.horizontalNormalizedPosition;

            if (Mathf.Abs(swipe) > _swipeThreshold) 
                Swipe(swipe);
            else
                MoveToCurrentIndex();

            _autoSwipe.ResetTime();
        }

        private void Swipe(float swipe)
        {
            ChangeBannerIndex(swipe);
            _toggles.SetToggleOn(_currentIndex - 1);
                
            MoveToCurrentIndex();
        }

        private void ChangeBannerIndex(float swipe)
        {
            if (swipe > 0)
            {
                if (_currentIndex - 1 >= 0)
                    _currentIndex--;
                else
                    _currentIndex = _positions.Length - 1;
            }
            else if(swipe < 0)
            {
                if (_currentIndex + 1 < _positions.Length)
                    _currentIndex++;
                else
                    _currentIndex = 0;
            }
            
            if (_currentIndex == 0)
            {
                _scrollRect.horizontalNormalizedPosition += _distance * (_positions.Length - 2);
                _currentIndex = _banners.Length - 2;
            }
            else if (_currentIndex == _positions.Length - 1)
            {
                _scrollRect.horizontalNormalizedPosition -= _distance * (_positions.Length - 2);
                _currentIndex = 1;
            }
        }
        
        private void MoveToCurrentIndex()
        {
            DOTween.To(
                    () => _scrollRect.horizontalNormalizedPosition,
                    x => _scrollRect.horizontalNormalizedPosition = x,
                    _positions[_currentIndex],
                    _swipeDuration
                )
                .SetEase(Ease.OutQuad)
                .SetId(transform);
        }
        
        private void OnSwipeTime()
        {
            Swipe(-1);
        }
    }
}