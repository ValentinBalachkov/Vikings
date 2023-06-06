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
                    if (_storageData[i].buildingData.StorageData.CurrentLevel > 0 ||
                        _storageData[i].buildingData.isSetOnMap)
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

            foreach (var character in _charactersOnMap.CharactersList)
            {
                UpdateCurrentBuilding(character);
            }
        }

        public void SetStorageUpgradeState(ItemData itemData)
        {
            var storage =
                _storageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == itemData.ID);

            foreach (var character in _charactersOnMap.CharactersList)
            {
                character.currentStorageToUpgrade = storage;
                UpdateCurrentBuilding(character);
            }
        }

        public void SetCraftingTableToUpgrade()
        {
            foreach (var character in _charactersOnMap.CharactersList)
            {
                character.currentStorageToUpgrade = _craftingTableController;
                UpdateCurrentBuilding(character);
            }
        }

        public CraftingTableController GetCraftingTable()
        {
            if (_craftingTableController == null)
            {
                return _craftingTableControllerOnStartGame;
            }

            return _craftingTableController;
        }

        public void UpdateCurrentBuilding(CharacterStateMachine character, bool isCraftTable = false,
            bool isItemCalling = false)
        {
            if (isItemCalling && character.CurrentState is CraftingState || isItemCalling && character.itemQueue.Count > 0)
            {
                return;
            }

            if (character.currentBuilding != null &&
                character.currentBuilding is BuildingController or CraftingTableController &&
                character.currentBuilding.IsFullStorage() &&
                character.CurrentState is not CraftingState || (character.currentBuilding != null &&
                                                                character.currentBuilding.isUpgradeState &&
                                                                character.currentBuilding.IsFullStorage()))
            {
                character.SetState<CraftingState>();
                return;
            }

            if (isCraftTable)
            {
                character.currentBuilding = GetCraftingTable();
                if (_craftingTableController == null)
                {
                    (character.currentBuilding as CraftingTableController).Init(_craftingTableDefault.buildingData);
                }
            }
            else if (_storageControllers.Any(x => x.isUpgradeState))
            {
                character.currentBuilding = _storageControllers.FirstOrDefault(x => x.isUpgradeState);
            }
            else
            {
                character.currentBuilding =
                    _buildingControllers.FirstOrDefault(x => x is CraftingTableController && !x.IsFullStorage());
                if (character.currentBuilding == null)
                {
                    character.currentBuilding =
                        _buildingControllers.FirstOrDefault(x => !x.IsFullStorage() && x is BuildingController);
                    if (character.currentBuilding == null)
                    {
                        character.currentBuilding = _buildingControllers.FirstOrDefault(x => !x.IsFullStorage());
                    }
                }
            }

            if (character.currentStorageToUpgrade != null)
            {
                character.currentStorageToUpgrade.SetUpgradeState();
                character.currentBuilding = character.currentStorageToUpgrade;
                character.currentStorageToUpgrade = null;
            }

            character.itemQueue.Clear();

            if (character.currentBuilding == null)
            {
                character.SetState<IdleState>();
                return;
            }

            UpdateItemsQueue(character.currentBuilding.GetCurrentPriceToUpgrades(), character);
        }

        private void UpdateItemsQueue(PriceToUpgrade[] priceToUpgrades, CharacterStateMachine character)
        {
            character.itemQueue.Clear();
            foreach (var price in priceToUpgrades)
            {
                var item = _itemsOnMapController.ItemsList.Where(x => x.Item.ID == price.itemData.ID && x.IsEnable);
                var storages = _storageControllers.Where(x =>
                    x.BuildingData.StorageData.ItemType.ID == price.itemData.ID &&
                    x.BuildingData != character.currentBuilding.BuildingData &&
                    x.IsAvailableToGetItem());
                character.itemQueue.AddRange(item);
                character.itemQueue.AddRange(storages);
            }


            if (character.itemQueue.Count > 0)
            {
                character.SetState<MovingState>();
                return;
            }

            character.SetState<IdleState>();
        }

        public Transform GetCurrentBuildingPosition(CharacterStateMachine character)
        {
            return character.currentBuilding.transform;
        }

        public AbstractBuilding GetCurrentBuilding(CharacterStateMachine character)
        {
            return character.currentBuilding;
        }

        public void SetItemToStorage(CharacterStateMachine characterStateMachine)
        {
            if (characterStateMachine.currentBuilding != null)
            {
                characterStateMachine.currentBuilding.ChangeStorageCount(
                    characterStateMachine.InventoryController.SetItemToStorage());
                UpdateCurrentBuilding(characterStateMachine);
            }
        }

        public IGetItem GetElementPosition(Transform playerPos, CharacterStateMachine character)
        {
            var items = character.itemQueue.OrderBy(x => x.Priority).ThenByDescending(x => x.GetItemData().DropCount)
                .ThenBy(x => Vector3.Distance(x.GetItemPosition().position, playerPos.position)).ToList();
            foreach (var item in items)
            {
                DebugLogger.SendMessage($"{item.GetItemData().ItemName}", Color.red);
                if (item.EnableToGet)
                {
                    item.EnableToGet = false;
                    return item;
                }
            }
            character.currentBuilding = null;
            character.currentStorageToUpgrade = null;
            character.itemQueue.Clear();
            character.SetState<IdleState>();
            return null;
        }

        public void ClearCurrentBuilding()
        {
            foreach (var character in _charactersOnMap.CharactersList)
            {
                character.currentBuilding = null;
            }
            
            _menu.EnableButtons(true);
        }

        public void OffCraftingStateAllCharacters(bool isCanCrafting)
        {
            foreach (var character in _charactersOnMap.CharactersList)
            {
                character.SetCraftingStateOff(isCanCrafting);
            }
        }

        public void UpgradeBuildingToStorage(CharacterStateMachine character)
        {
            _menu.EnableButtons(true);
            var data = _storageData.FirstOrDefault(x => x.buildingData == character.currentBuilding.BuildingData);
            if (data == null) return;
            if (data.buildingData.StorageData == null)
            {
                _craftingTableController = Instantiate(data.buildingData.CraftingTableController, data.spawnPoint);
                _buildingControllers.Add(_craftingTableController);
                data.buildingData.IsBuild = true;
                _craftingTableController.Init(_craftingTable);
            }
            else if (character.currentBuilding.isUpgradeState)
            {
                character.currentBuilding.UpgradeStorage();

                foreach (var c in _charactersOnMap.CharactersList)
                {
                    c.currentBuilding = null;
                    UpdateCurrentBuilding(c);
                }
                

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

            Destroy(character.currentBuilding.gameObject);

            _buildingControllers.Remove(data.buildingData.BuildingController);
            
            foreach (var c in _charactersOnMap.CharactersList)
            {
                DebugLogger.SendMessage("WE DID IT", Color.green);
            
                c.currentBuilding = null;
                UpdateCurrentBuilding(c);
            }
        }
    }

    [Serializable]
    public class StoragePosition
    {
        public BuildingData buildingData;
        public Transform spawnPoint;
        public ParticleSystem buildingParticle;
    }
}