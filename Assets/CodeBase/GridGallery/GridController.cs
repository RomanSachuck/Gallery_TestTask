using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Popups;
using CodeBase.TabBar;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.GridGallery
{
    public class GridController
    {
        private readonly GameObject _gridElementPrefab;
        private readonly Transform _gridParent;
        private readonly ImageDownloader _imageDownloader;
        private readonly GridView _gridView;
        private readonly TabBarView _tabBarView;
        private readonly PopupFactory _popupFactory;

        private Dictionary<GridElement, int> _gridElements;
        
        public GridController(GameObject gridElementPrefab, Transform gridParent, 
            ImageDownloader imageDownloader, GridView gridView,
            TabBarView tabBarView, PopupFactory popupFactory)
        {
            _gridElementPrefab = gridElementPrefab;
            _gridParent = gridParent;
            _imageDownloader = imageDownloader;
            _gridView = gridView;
            _tabBarView = tabBarView;
            _popupFactory = popupFactory;
        }

        public void Start()
        {
            CreateGrid();

            _tabBarView.SelectedFilterChanged += OnSelectedFilterChanged;
        }

        public void Destroy()
        {
            foreach (GridElement gridElement in _gridElements.Keys)
            {
                gridElement.EnabledWithoutImage -= OnGridElementEnabledWithoutImage;
                gridElement.ButtonClicked -= OnGridElementButtonClicked;
            }
            
            _tabBarView.SelectedFilterChanged -= OnSelectedFilterChanged;
        }
        
        private async Task CreateGrid()
        {
            _gridElements = new();

            int startImageCount = 10;
            int imageNumber = 1;

            for (; imageNumber <= startImageCount; imageNumber++)
            {
                CreateGridElement(imageNumber);
            }
            
            while (await _imageDownloader.IsImageExists(imageNumber))
            {
                CreateGridElement(imageNumber);
                imageNumber++;
            }
        }

        private void CreateGridElement(int imageNumber)
        {
            GridElement gridElement = Object.Instantiate(_gridElementPrefab, _gridParent)
                .GetComponent<GridElement>();
            
            gridElement.gameObject.SetActive(DetermineFilterStatus(imageNumber));
            gridElement.SetPremiumBadgeActive(DeterminePremiumStatus(imageNumber));
            gridElement.EnabledWithoutImage += OnGridElementEnabledWithoutImage;
            gridElement.ButtonClicked += OnGridElementButtonClicked;
            
            _gridElements.Add(gridElement, imageNumber);
            _gridView.AddNewElement(gridElement);
        }

        private bool DetermineFilterStatus(int elementNumber)
        {
            if(_tabBarView.SelectedFilter == TabFilterType.All)
                return true;
            
            return _tabBarView.SelectedFilter == TabFilterType.Even 
                    ? elementNumber % 2 == 0
                    : elementNumber % 2 != 0;
        }
        
        private bool DeterminePremiumStatus(int imageNumber)
        {
            return imageNumber % 4 == 0;
        }
        
        private void OnGridElementEnabledWithoutImage(GridElement gridElement)
        {
            gridElement.WaitAndInstallImage(_imageDownloader
                .GetSpriteForNumber(_gridElements[gridElement]));
        }
        
        private void OnGridElementButtonClicked(GridElement gridElement)
        {
            _popupFactory.CreatePopup(DeterminePremiumStatus(_gridElements[gridElement]),
                _imageDownloader.GetSpriteForNumber(_gridElements[gridElement]));
        }

        private void OnSelectedFilterChanged()
        {
            foreach (KeyValuePair<GridElement, int> gridElement in _gridElements) 
                gridElement.Key.gameObject.SetActive(DetermineFilterStatus(gridElement.Value));
            
            _gridView.PlayChangeFilterAnimation();
        }
    }
}