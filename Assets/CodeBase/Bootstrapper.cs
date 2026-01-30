using CodeBase.GridGallery;
using CodeBase.Popups;
using CodeBase.TabBar;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Transform _gridParent;
        [SerializeField] private Transform _popupParent;
        [SerializeField] private GameObject _gridElementPrefab;
        [SerializeField] private GameObject _popupSimplePrefab;
        [SerializeField] private GameObject _popupPremiumPrefab;
        [FormerlySerializedAs("_scrollViewCulling")] [SerializeField] private GridView _gridView;
        [SerializeField] private TabBarView _tabBarView;
        
        private GridController _gridController;
        private PopupFactory _popupFactory;

        private void Start()
        {
            ImageDownloader imageDownloader = new ImageDownloader();
            
            _popupFactory = new PopupFactory(_popupParent, _popupSimplePrefab, _popupPremiumPrefab);
            
            _gridController = new GridController(_gridElementPrefab, _gridParent, 
                imageDownloader, _gridView, _tabBarView, _popupFactory);
            
            _gridController.Start();
        }

        private void Update()
        {
            _popupFactory.Tick();
        }

        private void OnDestroy()
        {
            _gridController.Destroy();
        }
    }
}