using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Items;
using Vikings.UI;
using Vikings.Weapon;

namespace Vikings.Building
{
    public class BuildingsOnMap : MonoBehaviour
    {
        [SerializeField] private ItemsOnMapController _itemsOnMapController;
        [SerializeField] private StoragePosition[] _storageData;
        [SerializeField] private CharactersOnMap _charactersOnMap;

        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private MenuButtonsManager _menu;

        [SerializeField] private BuildingData _craftingTable;

        [SerializeField] private StoragePosition _craftingTableDefault;
        [SerializeField] private CraftingTableData _craftingTableData;
        [SerializeField] private WeaponsOnMapController _weaponsOnMapController;

        private List<AbstractBuilding> _buildingControllers = new();
        private List<StorageController> _storageControllers = new();

        private List<IGetItem> _itemQueue = new();
        private AbstractBuilding _currentBuilding;
        private CraftingTableController _craftingTableController;
        private CraftingTableController _craftingTableControllerOnStartGame;
        private AbstractBuilding _currentStorageToUpgrade;

        private void Start()
        {
            var storageData = _craftingTableDefault;
            _craftingTableControllerOnStartGame = Instantiate(storageData.buildingData.CraftingTableController,
                storageData.spawnPoint);
            _buildingControllers.Add(_craftingTableControllerOnStartGame);
            _craftingTableControllerOnStartGame.Model.SetActive(false);

            for (int i = 0; i < _storageData.Length; i++)
            {
                if (_storageData[i].buildingData.StorageData != null)
                {
                    if (_storageData[i].buildingData.StorageData.CurrentLevel > 0 || _storageData[i].buildingData.isSetOnMap)
                    {
                        SpawnStorage(i, true);
                    }
                }
                else
                {
                    if (_craftingTableData.currentLevel > 0)
                    {
                        SpawnStorage(i, true);
                    }
                }
            }
            
            _weaponsOnMapController.CheckForCraft();
        }

        public void SpawnStorage(int index, bool isSave = false)
        {
            if (_storageData[index].buildingData.IsBuild)
            {
                if (_storageData[index].buildingData.StorageData == null)
                {
                    _craftingTableController = Instantiate(_storageData[index].buildingData.CraftingTableController,
                        _storageData[index].spawnPoint);
                    _buildingControllers.Add(_craftingTableController);
                    Destroy(_craftingTableControllerOnStartGame.gameObject);
                    _buildingControllers.Remove(_craftingTableControllerOnStartGame);
                    return;
                }

                var item = Instantiate(_storageData[index].buildingData.StorageData.StorageController,
                    _storageData[index].spawnPoint);
                _buildingControllers.Add(item);
                item.Init(_storageData[index].buildingData, isSave);
                _inventoryView.AddStorageController(item);
                _storageControllers.Add(item);
            }
            else
            {
                var item = Instantiate(_storageData[index].buildingData.BuildingController,
                    _storageData[index].spawnPoint);
                _buildingControllers.Add(item);
                item.Init(_storageData[index].buildingData, isSave);

                if (index == 3)
                {
                    if (_craftingTableControllerOnStartGame != null)
                    {
                        _buildingControllers.Remove(_craftingTableControllerOnStartGame);
                        Destroy(_craftingTableControllerOnStartGame.gameObject);
                    }
                    
                }
            }

            UpdateCurrentBuilding();
        }

        public void SetStorageUpgradeState(ItemData itemData)
        {
            var storage =
                _storageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == itemData.ID);
            _currentStorageToUpgrade = storage;
        }

        public void SetCraftingTableToUpgrade()
        {
            _currentStorageToUpgrade = _craftingTableController;
        }

        public CraftingTableController GetCraftingTable()
        {
            if (_craftingTableController == null)
            {
                return _craftingTableControllerOnStartGame;
            }

            return _craftingTableController;
        }

        public void UpdateCurrentBuilding(bool isCraftTable = false, bool isItemCalling = false)
        {
            foreach (var character in _charactersOnMap.CharactersList)
            {
                if (isItemCalling && character.CurrentState is CraftingState || isItemCalling && _itemQueue.Count > 0)
                {
                    continue;
                }

                if (_currentBuilding != null && _currentBuilding is BuildingController or CraftingTableController &&
                    _currentBuilding.IsFullStorage() &&
                    character.CurrentState is not CraftingState || (_currentBuilding != null &&
                                                                    _currentBuilding.isUpgradeState &&
                                                                    _currentBuilding.IsFullStorage()))
                {
                    character.SetState<CraftingState>();
                    continue;
                }

                if (isCraftTable)
                {
                    _currentBuilding = GetCraftingTable();
                    if (_craftingTableController == null)
                    {
                        (_currentBuilding as CraftingTableController).Init(_craftingTableDefault.buildingData);
                    }
                }
                else if (_storageControllers.Any(x => x.isUpgradeState))
                {
                    _currentBuilding = _storageControllers.FirstOrDefault(x => x.isUpgradeState);
                }
                else
                {
                    _currentBuilding =
                        _buildingControllers.FirstOrDefault(x => x is CraftingTableController && !x.IsFullStorage());
                    if (_currentBuilding == null)
                    {
                        _currentBuilding = _buildingControllers
                            .OrderBy(x => x is BuildingController)
                            .FirstOrDefault(x => !x.IsFullStorage());
                    }
                }

                if (_currentStorageToUpgrade != null)
                {
                    _currentStorageToUpgrade.SetUpgradeState();
                    _currentBuilding = _currentStorageToUpgrade;
                    _currentStorageToUpgrade = null;
                }

                _itemQueue.Clear();

                if (_currentBuilding == null)
                {
                    character.SetState<IdleState>();
                    continue;
                }

                UpdateItemsQueue(_currentBuilding.GetCurrentPriceToUpgrades());
            }
        }

        private void UpdateItemsQueue(PriceToUpgrade[] priceToUpgrades)
        {
            _itemQueue.Clear();
            foreach (var price in priceToUpgrades)
            {
                var item = _itemsOnMapController.ItemsList.Where(x => x.Item.ID == price.itemData.ID && x.IsEnable);
                var storages = _storageControllers.Where(x =>
                    x.BuildingData.StorageData.ItemType.ID == price.itemData.ID &&
                    x.BuildingData != _currentBuilding.BuildingData &&
                    x.IsAvailableToGetItem());
                _itemQueue.AddRange(item);
                _itemQueue.AddRange(storages);
            }

            foreach (var character in _charactersOnMap.CharactersList)
            {
                if (_itemQueue.Count > 0)
                {
                    character.SetState<MovingState>();
                    continue;
                }

                character.SetState<IdleState>();
            }
        }

        public Transform GetCurrentBuildingPosition()
        {
            return _currentBuilding.transform;
        }

        public AbstractBuilding GetCurrentBuilding()
        {
            return _currentBuilding;
        }

        public void SetItemToStorage(CharacterStateMachine characterStateMachine)
        {
            if (_currentBuilding != null)
            {
                _currentBuilding.ChangeStorageCount(characterStateMachine.InventoryController.SetItemToStorage());
                UpdateCurrentBuilding();
            }
        }

        public IGetItem GetElementPosition()
        {
            return _itemQueue.OrderBy(x => x.Priority).ThenByDescending(x => x.GetItemData().DropCount).ToList()[0];
        }

        public void ClearCurrentBuilding()
        {
            _currentBuilding = null;
        }

        public void UpgradeBuildingToStorage()
        {
            _menu.EnableButtons(true);
            var data = _storageData.FirstOrDefault(x => x.buildingData == _currentBuilding.BuildingData);
            if (data == null) return;

            if (data.buildingData.StorageData == null)
            {
                _craftingTableController = Instantiate(data.buildingData.CraftingTableController, data.spawnPoint);
                _buildingControllers.Add(_craftingTableController);
                data.buildingData.IsBuild = true;
                _craftingTableController.Init(_craftingTable);
            }
            else if (_currentBuilding.isUpgradeState)
            {
                _currentBuilding.UpgradeStorage();
                _currentBuilding = null;
                UpdateCurrentBuilding();
                return;
            }
            else
            {
                var item = Instantiate(data.buildingData.StorageData.StorageController, data.spawnPoint);

                _storageControllers.Add(item);
                _buildingControllers.Add(item);
                item.Init(data.buildingData);
                data.buildingData.IsBuild = true;
                _inventoryView.AddStorageController(item);
            }

            data.buildingData.BuildingController.OnChangeCount = null;

            Destroy(_currentBuilding.gameObject);

            _currentBuilding = null;

            _buildingControllers.Remove(data.buildingData.BuildingController);

            UpdateCurrentBuilding();
        }
    }

    [Serializable]
    public class StoragePosition
    {
        public BuildingData buildingData;
        public Transform spawnPoint;
    }
}