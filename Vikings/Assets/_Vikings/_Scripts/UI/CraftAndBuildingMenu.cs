using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using PanelManager.Scripts.Panels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vikings.Building;
using Vikings.Map;
using Vikings.Object;
using Vikings.Weapon;

namespace Vikings.UI
{
    public class CraftAndBuildingMenu : ViewBase, IAcceptArgs<MapFactory, CharactersTaskManager>
    {
        public override PanelType PanelType => PanelType.Screen;
        public override bool RememberInHistory => false;
        
        [SerializeField] private Transform _content;
        [SerializeField] private MenuElement _menuElement;

        [SerializeField] private List<StorageData> _storageDatas;
      //  [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private CraftingTableData _craftingTableData;
       // [SerializeField] private BuildingData _craftingTableBuildingData;
        [SerializeField] private WeaponsOnMapController _weaponsOnMapController;
        [SerializeField] private AudioSource _audioSourceBtnClick;

        [SerializeField] private Button _closeButton;

        private List<MenuElement> _menuElements = new();

        private MapFactory _mapFactory;
        private CharactersTaskManager _charactersTaskManager;
        

        public void AcceptArg(MapFactory arg, CharactersTaskManager arg2)
        {
            _mapFactory = arg;
            _charactersTaskManager = arg2;
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            CreatePanels();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _panelManager.ActiveOverlay(true);
                _panelManager.ClosePanel<CraftAndBuildingMenu>();
            }).AddTo(_panelManager.Disposable);
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            ClearPanel();
        }

        private void CreatePanels()
        {
            var storages = _mapFactory.GetAllBuildings<Storage>();
            
            foreach (var building in storages)
            {
                var item = Instantiate(_menuElement, _content);
                item.UpdateUI(building);
                item.AddOnClickListener(() =>
                {
                    _charactersTaskManager.setBuildingToQueue.Execute(building);
                    _panelManager.ClosePanel<CraftAndBuildingMenu>();
                });
                var cortege = building.IsEnableToBuild(_mapFactory.GetAllBuildings<CraftingTableController>());
                item.SetEnable(cortege.Item1, cortege.Item2);
                _menuElements.Add(item);
            }

            SortElements();
        }

        private void ClearPanel()
        {
            foreach (var element in _menuElements)
            {
                Destroy(element.gameObject);
            }
            _menuElements.Clear();
        }
        

        private void SortElements()
        {
            foreach (var element in _menuElements.OrderByDescending(x => x.priority))
            {
                element.transform.SetAsFirstSibling();
            }
        }


        private void Spawn()
        {
            for (int i = 0; i < _storageDatas.Count; i++)
            {
                int k = 1;
                if (i == 0)
                {
                    k = 0;
                }

                var item = Instantiate(_menuElement, _content);
                _menuElements.Add(item);
                item.UpdateUI(_storageDatas[i].nameText, _storageDatas[i].description, _storageDatas[i].CurrentLevel, _storageDatas[i].icon,
                    _storageDatas[i].PriceToUpgrade.ToArray(),_storageDatas[i].priority);
                item.SetButtonDescription(_storageDatas[i].CurrentLevel == 0);
                string requiredText =
                    $"REQUIRED:  {_storageDatas[i].required} LEVEL{_craftingTableData.currentLevel + 1}";
                if (i == 2 && _storageDatas[i].CurrentLevel >= 5)
                {
                    requiredText = "MAX LEVEL";
                }
                item.SetEnable((_craftingTableData.currentLevel - _storageDatas[i].CurrentLevel >= k) ||
                               (_storageDatas[i].isDefaultOpen && _storageDatas[i].CurrentLevel == 0 ||
                                ((i == 2 && _storageDatas[i].CurrentLevel < 5) && (_craftingTableData.currentLevel - _storageDatas[i].CurrentLevel >= k))),
                    requiredText);
                var index = i;
                if (_storageDatas[i].CurrentLevel == 0)
                {
                    item.AddOnClickListener(() =>
                    {
                        _audioSourceBtnClick.Play();
                       // _buildingsOnMap.SpawnStorage(index);
                        gameObject.SetActive(false);
                    });
                }
                else
                {
                    item.AddOnClickListener(() =>
                    {
                        _audioSourceBtnClick.Play();
                       // _buildingsOnMap.SetStorageUpgradeState(_storageDatas[index].ItemType);
                        gameObject.SetActive(false);
                    });
                }
            }


            var craftingTable = Instantiate(_menuElement, _content);
            _menuElements.Add(craftingTable);
            craftingTable.UpdateUI(_craftingTableData.nameText, _craftingTableData.description, _craftingTableData.currentLevel,
                _craftingTableData.icon, _craftingTableData.PriceToUpgrade.ToArray(), _craftingTableData.priority);
            craftingTable.SetButtonDescription(_craftingTableData.currentLevel == 0);
            craftingTable.SetEnable(_weaponsOnMapController.WeaponsData[0].IsOpen, $"REQUIRED:  {_craftingTableData.required} LEVEL{1}");
            if (_craftingTableData.currentLevel == 0)
            {
                craftingTable.AddOnClickListener(() =>
                {
                    _audioSourceBtnClick.Play();
                   // _buildingsOnMap.SpawnStorage(3);
                    gameObject.SetActive(false);
                });
            }
            else
            {
                craftingTable.AddOnClickListener(() =>
                {
                    _audioSourceBtnClick.Play();
                   // _buildingsOnMap.SetCraftingTableToUpgrade();
                    gameObject.SetActive(false);
                });
            }


            for (int i = 0; i < _weaponsOnMapController.WeaponsData.Count; i++)
            {
                var weapon = Instantiate(_menuElement, _content);
                _menuElements.Add(weapon);
                weapon.UpdateUI(_weaponsOnMapController.WeaponsData[i].nameText,
                    _weaponsOnMapController.WeaponsData[i].description, _weaponsOnMapController.WeaponsData[i].level,
                    _weaponsOnMapController.WeaponsData[i].icon,
                    _weaponsOnMapController.WeaponsData[i].PriceToBuy.ToArray(), _weaponsOnMapController.WeaponsData[i].priority);
                weapon.SetButtonDescription(!_weaponsOnMapController.WeaponsData[i].IsOpen);
                if (i == 0)
                {
                    if (_weaponsOnMapController.WeaponsData[i].IsOpen)
                    {
                        weapon.SetEnable(_storageDatas[0].CurrentLevel > 0 && _craftingTableData.currentLevel > _weaponsOnMapController.WeaponsData[i].level,
                            $"REQUIRED:  {_weaponsOnMapController.WeaponsData[i].required} LEVEL{_craftingTableData.currentLevel + 1}");
                    }
                    else
                    {
                        weapon.SetEnable(_storageDatas[0].CurrentLevel > 0 && !_weaponsOnMapController.WeaponsData[i].IsOpen,
                            $"REQUIRED:  {_weaponsOnMapController.WeaponsData[i].required} LEVEL{_storageDatas[0].CurrentLevel + 1}");
                    }
                }
                else
                {
                    if (_weaponsOnMapController.WeaponsData[i].IsOpen)
                    {
                        weapon.SetEnable(_craftingTableData.currentLevel >= 2 && _craftingTableData.currentLevel > _weaponsOnMapController.WeaponsData[i].level, 
                            $"REQUIRED:  {_weaponsOnMapController.WeaponsData[i].required} LEVEL{_craftingTableData.currentLevel + 1}");
                    }
                    else
                    {
                        weapon.SetEnable(_craftingTableData.currentLevel >= 2 && !_weaponsOnMapController.WeaponsData[i].IsOpen, 
                            $"REQUIRED:  {_weaponsOnMapController.WeaponsData[i].required} LEVEL{2}");
                    }
                    
                }
                
                var index = i;
                weapon.AddOnClickListener(() =>
                {
                    _audioSourceBtnClick.Play();
                    _weaponsOnMapController.StartCraftWeapon(index);
                    gameObject.SetActive(false);
                });
            }
        }
    }
}