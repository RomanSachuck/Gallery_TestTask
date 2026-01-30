using System;
using CodeBase.Popups.PremiumLogic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Popups
{
    public class PopupPremium : PopupBase
    {
        public event Action RecoveryButtonClicked;
        public event Action PrivacyButtonClicked;
        public event Action TermsButtonClicked;
        public event Action<PriceType> SelectedPriceChanged;

        [SerializeField] private PriceButtonsController _priceButtons;
        [SerializeField] private Button _recoveryButton;
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _termsButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _priceButtons.SelectedPriceChanged += OnSelectedPriceChanged;
            _recoveryButton.onClick.AddListener(OnRecoveryButtonClicked);
            _privacyButton.onClick.AddListener(OnPrivacyButtonClicked);
            _termsButton.onClick.AddListener(OnTermsButtonClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _priceButtons.SelectedPriceChanged -= OnSelectedPriceChanged;
            _recoveryButton.onClick.RemoveListener(OnRecoveryButtonClicked);
            _privacyButton.onClick.RemoveListener(OnPrivacyButtonClicked);
            _termsButton.onClick.RemoveListener(OnTermsButtonClicked);
        }

        private void OnTermsButtonClicked()
        {
            TermsButtonClicked?.Invoke();
            _termsButton.transform.DoClickShake(Vector3.one);
        }

        private void OnPrivacyButtonClicked()
        {
            PrivacyButtonClicked?.Invoke();
            _privacyButton.transform.DoClickShake(Vector3.one);
        }

        private void OnRecoveryButtonClicked()
        {
            RecoveryButtonClicked?.Invoke();
            _recoveryButton.transform.DoClickShake(Vector3.one);
        }

        private void OnSelectedPriceChanged(PriceType privateType)
        {
            SelectedPriceChanged?.Invoke(privateType);
        }
    }
}