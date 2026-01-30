using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Popups
{
    public class PopupSimple : PopupBase
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private Sprite _defaultSprite;
        
        public async void WaitAndInstallImage(Task<Sprite> loadingTask)
        {
            _image.sprite = _defaultSprite;
            _loadingIndicator.SetActive(true);
            
            _image.sprite = await loadingTask;
            
            _loadingIndicator.SetActive(false);
        }
    }
}