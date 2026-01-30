using System;
using UnityEngine;

namespace CodeBase.Popups.PremiumLogic
{
    public class PriceButtonsController : MonoBehaviour
    {
        public event Action<PriceType> SelectedPriceChanged;
        
        [SerializeField] private PriceButton[] _buttons;

        private PriceType _selectedPrice = PriceType.Year;
        
        private void OnEnable()
        {
            foreach (PriceButton button in _buttons) 
                button.ButtonClicked += OnClickButton;
        }

        private void OnDisable()
        {
            foreach (PriceButton button in _buttons) 
                button.ButtonClicked -= OnClickButton;
        }

        private void OnClickButton(PriceType priceType)
        {
            if (priceType != _selectedPrice)
            {
                foreach (PriceButton button in _buttons) 
                    button.SetSelect(priceType == button.PriceType);
                
                _selectedPrice = priceType;
                SelectedPriceChanged?.Invoke(_selectedPrice);
            }
        }
    }
}