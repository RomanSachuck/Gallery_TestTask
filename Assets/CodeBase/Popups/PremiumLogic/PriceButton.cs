using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Popups.PremiumLogic
{
    public class PriceButton : MonoBehaviour
    {
        public event Action<PriceType> ButtonClicked;
        
        [SerializeField] private Button _button;
        [SerializeField] private Image _circleImage;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _saveText;
        [SerializeField] private Transform _animatedContainer;

        [SerializeField] private Color _selectedPriceTextColor;
        [SerializeField] private Color _selectedSaveTextColor;
        [SerializeField] private Color _unselectedPriceTextColor;
        [SerializeField] private Color _unselectedSaveTextColor;
        [SerializeField] private Sprite _selectedCircleSprite;
        [SerializeField] private Sprite _unselectedCircleSprite;
        
        private Vector3 _animatedContainerStartScale;

        [field:SerializeField] public PriceType PriceType { get; private set; }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickButton);
        }

        private void Awake()
        {
            _animatedContainerStartScale = _animatedContainer.localScale;
        }

        public void SetSelect(bool isSelect)
        {
            if (isSelect)
            {
                _priceText.color = _selectedPriceTextColor;
                
                if(_saveText != null)
                    _saveText.color = _selectedSaveTextColor;
                
                _circleImage.sprite = _selectedCircleSprite;
            }
            else
            {
                _priceText.color = _unselectedPriceTextColor;
                
                if(_saveText != null)
                    _saveText.color = _unselectedSaveTextColor;
                
                _circleImage.sprite = _unselectedCircleSprite;
            }
        }
        
        private void OnClickButton()
        {
            ButtonClicked?.Invoke(PriceType);
            
            _animatedContainer.DoClickShake(_animatedContainerStartScale);
        }
    }
}