using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Banners
{
    public class Toggles : MonoBehaviour
    {
        [SerializeField] private Image[] _toggles;
        [SerializeField] private Sprite _toggleOnSprite;
        [SerializeField] private Sprite _toggleOffSprite;

        private int _currentIndex;

        public void SetToggleOn(int index)
        {
            _toggles[_currentIndex].sprite = _toggleOffSprite;
            _currentIndex = index;
            _toggles[_currentIndex].sprite = _toggleOnSprite;
        }
    }
}