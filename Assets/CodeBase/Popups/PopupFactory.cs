using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Popups
{
    public class PopupFactory
    {
        private readonly Transform _parent;
        private readonly GameObject _premiumPopupPrefab;
        private readonly GameObject _simplePopupPrefab;
        
        private PopupPremium _premiumPopup;
        private PopupSimple _simplePopup;
        
        private float _timeCounter;

        public PopupFactory(Transform parent, GameObject simplePopupPrefab, GameObject premiumPopupPrefab)
        {
            _parent = parent;
            _simplePopupPrefab = simplePopupPrefab;
            _premiumPopupPrefab = premiumPopupPrefab;
        }

        public void Tick()
        {
            if(_premiumPopup != null)
                _premiumPopup?.Tick();
            
            if(_simplePopup != null)
                _simplePopup.Tick();
        }
        
        public void CreatePopup(bool isPremium, Task<Sprite> spriteTask)
        {
            if(isPremium)
                CreatePremiumPopup();
            else
                CreateSimplePopup(spriteTask);
        }

        private void CreateSimplePopup(Task<Sprite> spriteTask)
        {
            if (_simplePopup == null)
                _simplePopup = Object.Instantiate(_simplePopupPrefab, _parent).GetComponent<PopupSimple>();
            
            _simplePopup.Open();
            _simplePopup.WaitAndInstallImage(spriteTask);
        }

        private void CreatePremiumPopup()
        {
            if (_premiumPopup == null)
                _premiumPopup = Object.Instantiate(_premiumPopupPrefab, _parent).GetComponent<PopupPremium>();
            
            _premiumPopup.Open();
        }
    }
}