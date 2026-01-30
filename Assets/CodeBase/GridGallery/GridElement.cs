using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GridGallery
{
    public class GridElement : MonoBehaviour
    {
        public event Action<GridElement> EnabledWithoutImage;
        public event Action<GridElement> ButtonClicked;
        
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _loadingIndication;
        [SerializeField] private GameObject _premiumBadge;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        private bool _imageIsSet;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void SetActiveState(bool isActive)
        {
            _content.SetActive(isActive);
            
            if(isActive && _imageIsSet == false)
                EnabledWithoutImage?.Invoke(this);
        }

        public async void WaitAndInstallImage(Task<Sprite> loadingTask)
        {
            _image.sprite = await loadingTask;
            _loadingIndication.SetActive(false);
            _imageIsSet = true;
        }

        public void SetPremiumBadgeActive(bool isActive)
        {
            _premiumBadge.SetActive(isActive);
        }
        
        private void OnButtonClicked()
        {
            ButtonClicked?.Invoke(this);
        }
    }
}
