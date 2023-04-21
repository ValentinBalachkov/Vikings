using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Items;
using Vikings.UI;
using Random = UnityEngine.Random;

namespace Vikings.Building
{
    public class BuildingsOnMap : MonoBehaviour
    {
        [SerializeField] private ItemsOnMapController _itemsOnMapController;
        [SerializeField] private StoragePosition[] _storageData;
        [SerializeField] private CharactersOnMap _charactersOnMap;

        [SerializeField] private InventoryView _inventoryView;

        private List<AbstractBuilding> _buildingControllers = new();
        private List<StorageController> _storageControllers = new();

        private List<IGetItem> _itemQueue = new();
        private AbstractBuilding _currentBuilding;

        public void SpawnStorage(int index)
        {
            _storageData[index].buildingData.LoadData();
            if (_storageData[index].buildingData.IsBuild)
            {
                var item = Instantiate(_storageData[index].buildingData.StorageData.StorageController,
                    _storageData[index].spawnPoint);
                _buildingControllers.Add(item);
                item.Init(_storageData[index].buildingData);
                _inventoryView.AddStorageController(item);
                _storageControllers.Add(item);
            }
            else
            {
                var item = Instantiate(_storageData[index].buildingData.BuildingController,
                    _storageData[index].spawnPoint);
                _buildingControllers.Add(item);
                item.Init(_storageData[index].buildingData);
            }

            UpdateCurrentBuilding();
        }

        // public void SpawnStorages()
        // {
        //     foreach (var storage in _storageData)
        //     {
        //         storage.buildingData.LoadData();
        //         if (storage.buildingData.IsBuild)
        //         {
        //             var item = Instantiate(storage.buildingData.StorageData.StorageController, storage.spawnPoint);
        //             _buildingControllers.Add(item);
        //             item.Init(storage.buildingData);
        //             _inventoryView.AddStorageController(item);
        //         }
        //         else
        //         {
        //             var item = Instantiate(storage.buildingData.BuildingController, storage.spawnPoint);
        //             _buildingControllers.Add(item);
        //             item.Init(storage.buildingData);
        //         }
        //         
        //     }
        //
        //     UpdateCurrentBuilding();
        // }

        public void UpdateCurrentBuilding()
        {
            foreach (var character in _charactersOnMap.CharactersList)
            {
                if (_currentBuilding != null && _currentBuilding is BuildingController &&
                    _currentBuilding.IsFullStorage() &&
                    character.CurrentState is not CraftingState)
                {
                    character.SetState<CraftingState>();
                    continue;
                }

                _currentBuilding = _buildingControllers.OrderBy(x => x is BuildingController)
                    .FirstOrDefault(x => !x.IsFullStorage());
                _itemQueue.Clear();

                if (_currentBuilding == null)
                {
                    character.SetState<IdleState>();
                    continue;
                }

                UpdateItemsQueue(_currentBuilding.GetCurrentPriceToUpgrades());
            }
        }

        public void UpdateItemsQueue(PriceToUpgrade[] priceToUpgrades)
        {
            _itemQueue.Clear();
            foreach (var price in priceToUpgrades)
            {
                var item = _itemsOnMapController.ItemsList.Where(x => x.Item.ID == price.itemData.ID);
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
            return _itemQueue.OrderBy(x => x.Priority).ToList()[0];
        }

        public void UpgradeBuildingToStorage()
        {
            var data = _storageData.FirstOrDefault(x => x.buildingData == _currentBuilding.BuildingData);
            if (data == null) return;

            var item = Instantiate(data.buildingData.StorageData.StorageController, data.spawnPoint);

            _storageControllers.Add(item);

            _buildingControllers.Add(item);

            data.buildingData.BuildingController.OnChangeCount = null;

            Destroy(_currentBuilding.gameObject);

            _currentBuilding = null;

            _buildingControllers.Remove(data.buildingData.BuildingController);

            item.Init(data.buildingData);

            _inventoryView.AddStorageController(item);

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