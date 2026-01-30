using System;
using UnityEngine;

namespace CodeBase.Banners
{
    public class AutoSwipe : MonoBehaviour
    {
        public event Action SwipeTime;
        
        [SerializeField] private float _swipeTime = 5;

        private float _timeCounter;

        private void Update()
        {
            _timeCounter += Time.deltaTime;

            if (_timeCounter >= _swipeTime)
            {
                SwipeTime?.Invoke();
                _timeCounter = 0;
            }
        }

        public void ResetTime()
        {
            _timeCounter = 0;
        }
    }
}