using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Items;
using Random = UnityEngine.Random;


namespace Vikings.Building
{
    public class StorageOnMap : MonoBehaviour
    {
        public List<AbstractBuilding> StorageControllers => _storageControllers;

        [SerializeField] private ItemsOnMapController _itemsOnMapController;
        [SerializeField] private CharacterStateMachine _characterStateMachine;
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private StoragePosition[] _storageData;

        private List<AbstractBuilding> _storageControllers = new();
        private List<ItemController> _itemQueue = new();
        private ItemController _currentItem;
        private AbstractBuilding _currentStorage;

        public void SpawnStorages()
        {
            foreach (var storage in _storageData)
            {
                storage.buildingData.LoadData();
                if (storage.buildingData.IsBuild)
                {
                    var item = Instantiate(storage.buildingData.StorageData.StorageController, storage.spawnPoint);
                    _storageControllers.Add(item);
                    item.Init(storage.buildingData, this);
                    item.OnChangeCountStorage += ChangeCharacterState;
                }
                else
                {
                    var item = Instantiate(storage.buildingData.BuildingController, storage.spawnPoint);
                    _storageControllers.Add(item);
                    item.Init(storage.buildingData, this);
                    item.OnChangeCount += (ItemData) =>
                    {
                        ChangeCharacterState(_currentItem.Item);
                    };
                }
            }

            foreach (var item in _itemsOnMapController.ItemsList)
            {
                item.OnEnable += AddItemsOnQueue;
            }

            AddItemsOnQueue();
        }

        public void UpgradeBuildingToStorage(BuildingData buildingData)
        {
            var data = _storageData.FirstOrDefault(x => x.buildingData == buildingData);
            if (data == null) return;

            var item = Instantiate(data.buildingData.StorageData.StorageController, data.spawnPoint);
            _storageControllers.Add(item);
            data.buildingData.BuildingController.OnChangeCount = null;
            _storageControllers.Remove(data.buildingData.BuildingController);
            item.Init(data.buildingData, this);
            item.OnChangeCountStorage += ChangeCharacterState;
        }

        private void AddItemsOnQueue()
        {
            var emptyStorages = _storageControllers.Where(x => !x.IsFullStorage()).ToList();

            if (emptyStorages.Count != 0)
            {
                foreach (var storage in emptyStorages)
                {
                    _itemQueue.AddRange(_itemsOnMapController.ItemsList
                        .Where(x => x.Item.ID == storage.BuildingData.StorageData.ItemType.ID).ToList());
                }

                _characterStateMachine.SetState<MovingState>();
                return;
            }

            _characterStateMachine.SetState<IdleState>();
        }

        public ItemController GetElementPosition()
        {
            _currentItem = _itemQueue[Random.Range(0, _itemQueue.Count)];
            return _currentItem;
        }

        public ItemController GetElementFromQueue()
        {
            _currentStorage =
                _storageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == _currentItem.Item.ID);
            _itemQueue.Remove(_currentItem);
            return _currentItem;
        }

        public void SetItemToStorage()
        {
            _currentStorage =
                _storageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == _currentItem.Item.ID);
            if (_currentStorage != null) _currentStorage.ChangeStorageCount(_inventoryController.SetItemToStorage());
        }

        public Transform GetCurrentStoragePosition()
        {
            return _currentStorage.transform;
        }

        private void ChangeCharacterState(ItemData itemData)
        {
            var controllers = _storageControllers.Where(x => !x.IsFullStorage()).ToList();
            var controllersFullStorage = _storageControllers.Where(x => x.IsFullStorage()).ToList();

            foreach (var controller in controllersFullStorage)
            {
                var itemsList = _itemQueue.Where(x => x.Item.ID == controller.BuildingData.StorageData.ItemType.ID).ToList();
                foreach (var item in itemsList)
                {
                    _itemQueue.Remove(item);
                }
            }


            if (controllers.Count == 0)
            {
                _characterStateMachine.SetState<IdleState>();
                return;
            }

            var storage = _storageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == itemData.ID);
            if (storage == null || storage.IsFullStorage())
            {
                return;
            }

            _itemQueue.AddRange(_itemsOnMapController.ItemsList.Where(x => x.Item.ID == itemData.ID && x.IsEnable)
                .ToList());
            if (_itemQueue.Count == 0)
            {
                _characterStateMachine.SetState<IdleState>();
                return;
            }

            _characterStateMachine.SetState<MovingState>();
        }
    }

    [Serializable]
    public class StoragePosition
    {
        public BuildingData buildingData;
        public Transform spawnPoint;
    }
}