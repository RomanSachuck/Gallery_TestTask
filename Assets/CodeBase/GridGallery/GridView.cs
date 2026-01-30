using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GridGallery
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private List<GridElement> _elements;
        [SerializeField] private GridAnimator _gridAnimator;
        
        [SerializeField] private float _checkInterval = 0.1f;
        [SerializeField] private float _margin = 50f;

        private List<bool> _childStates;
        private float _lastCheckTime;

        private void Start()
        {
            _elements = new();
            _childStates = new();
        }

        private void Update()
        {
            if (Time.time - _lastCheckTime >= _checkInterval)
            {
                CheckVisibility();
                _lastCheckTime = Time.time;
            }
        }

        public void AddNewElement(GridElement element)
        {
            _elements.Add(element);
            _childStates.Add(false);
        }

        public void PlayChangeFilterAnimation()
        {
            _gridAnimator.PlayChangeFilterAnimation(_elements.Select(e => e.transform).ToArray());
        }
        
        private void CheckVisibility()
        {
            Rect scrollViewRect = GetScrollViewRect();

            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i] == null) continue;

                Rect childRect = GetWorldRect(_elements[i].transform as RectTransform);
                bool isVisible = scrollViewRect.Overlaps(childRect);
                
                if (isVisible != _childStates[i])
                {
                    _elements[i].SetActiveState(isVisible);
                    _childStates[i] = isVisible;
                }
            }
        }

        private Rect GetScrollViewRect()
        {
            Vector3[] corners = new Vector3[4];
            _scrollRect.viewport.GetWorldCorners(corners);
            
            float minX = corners[0].x - _margin;
            float minY = corners[0].y - _margin;
            float maxX = corners[2].x + _margin;
            float maxY = corners[2].y + _margin;

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        private Rect GetWorldRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Vector2 min = corners[0];
            Vector2 max = corners[2];

            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }
    }
}