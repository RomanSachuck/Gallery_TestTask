using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Popups
{
    public class PopupBase : MonoBehaviour
    {
        [SerializeField] private float _timeToDestroy = 10;
        [SerializeField] private float _openAnimationDuration = 0.5f;
        [SerializeField] private float _openAnimationDistance = 1000f;
        [SerializeField] private Transform _animatedContent;
        [SerializeField] private Button _closeButton;

        private float _timeCounter;
        private float _startContentPositionY;

        protected virtual void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
        }

        protected virtual void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
        }

        public virtual void Tick()
        {
            if (gameObject.activeSelf == false)
            {
                _timeCounter += Time.deltaTime;

                if (_timeCounter >= _timeToDestroy) 
                    Destroy(gameObject);
            }
        }

        public virtual void Open()
        {
            _startContentPositionY = transform.position.y;
            
            gameObject.SetActive(true);
            _animatedContent.DOMoveY(_startContentPositionY, _openAnimationDuration).
                From(_startContentPositionY - _openAnimationDistance)
                .SetEase(Ease.OutBack);

            _timeCounter = 0;
        }

        protected virtual void Close()
        {
            DOTween.Sequence()
                .Append(_animatedContent
                    .DOMoveY(_startContentPositionY - _openAnimationDistance, _openAnimationDuration)
                    .SetEase(Ease.InBack))
                .AppendCallback(() => gameObject.SetActive(false));
        }
    }
}