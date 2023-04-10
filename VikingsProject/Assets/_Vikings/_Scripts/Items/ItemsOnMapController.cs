using System.Collections.Generic;
using UnityEngine;
using Vikings.Building;
using Vikings.Inventory;

namespace Vikings.Items
{
    public class ItemsOnMapController : MonoBehaviour
    {
        public StorageController StorageController => _storageController;
        
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private ItemPosition[] _itemPositions;

        [SerializeField] private StorageData _storageData;
        [SerializeField] private Transform _storagePoint;

        private Queue<ItemController> _itemsQueue = new();

        private StorageController _storageController;


        public void AddElementToQueue(ItemController itemController)
        {
            _itemsQueue.Enqueue(itemController);
        }

        public ItemController GetElementPosition()
        {
            return _itemsQueue.Peek();
        }

        public ItemController GetElementFromQueue()
        {
            return _itemsQueue.Dequeue();
        }

        private void Awake()
        {
            Spawn();
        }

        private void Spawn()
        {
            foreach (var item in _itemPositions)
            {
                foreach (var pos in item.position)
                {
                    var itemOnScene = Instantiate(item.item.Prefab, pos);
                    itemOnScene.Init(item.item, _inventoryController);
                    _itemsQueue.Enqueue(itemOnScene);
                }
            }

            _storageController = Instantiate(_storageData.StorageController, _storagePoint);
        }
    }
}