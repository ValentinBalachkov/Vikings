using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Vikings.Building;
using Vikings.Chanacter;
using Vikings.Inventory;

namespace Vikings.Items
{
    public class ItemsOnMapController : MonoBehaviour
    {
        public StorageController StorageController => _storageController;
        public GameObject BoneFire => _boneFire;

        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private ItemPosition[] _itemPositions;

        [SerializeField] private StorageData _storageData;
        [SerializeField] private Transform _storagePoint;

        [SerializeField] private GameObject _boneFirePrefab;
        [SerializeField] private Transform _boneFirePoint;

        [SerializeField] private InventoryData _inventoryData;
        [SerializeField] private CharacterStateMachine _characterStateMachine;


        private List<ItemController> _itemsQueue = new();
        private List<ItemController> _removedObjectsPool = new();

        private StorageController _storageController;
        private GameObject _boneFire;


        private void Awake()
        {
            Spawn();
            _inventoryController.InventoryData.OnInventoryChange += SortQueueToItemsCount;
        }

        public void AddElementToQueue(ItemController itemController)
        {
            _itemsQueue.Add(itemController);
            Debug.Log(_itemsQueue.Count + " Add");
        }

        public ItemController GetElementPosition()
        {
            return _itemsQueue.Count > 0 ? _itemsQueue[0] : null;
        }

        public ItemController GetElementFromQueue()
        {
            var item = _itemsQueue[^1];
            _itemsQueue.RemoveAt(0);
            Debug.Log(item.Item.ID+ " Remove");
            return item;
        }

        private void SortQueueToItemsCount(ItemData itemData)
        {
            var inventory = _inventoryData.GetInventory();

            foreach (var item in inventory)
            {
                if (item.count >= item.itemData.LimitCount)
                {
                    _removedObjectsPool.AddRange(_itemsQueue.Where(x => x.Item.ID == item.itemData.ID));
                    _itemsQueue.RemoveAll(x => x.Item.ID == item.itemData.ID);
                }
                else
                {
                    _itemsQueue.AddRange(_removedObjectsPool.Where(x => x.Item.ID == item.itemData.ID));
                    _removedObjectsPool.RemoveAll(x => x.Item.ID == item.itemData.ID);
                }
            }
            
            Debug.Log(_itemsQueue.Count + " Count on collect");

            if (_itemsQueue.Count == 0)
            {
                if (!_storageController.StorageData.IsFullStorage())
                {
                    _characterStateMachine.SetState<MoveToStorageState>();
                    return;
                }

                _characterStateMachine.SetState<IdleState>();
            }
            else
            {
                _characterStateMachine.SetState<MovingState>();
            }
        }

        private void Spawn()
        {
            foreach (var item in _itemPositions)
            {
                foreach (var pos in item.position)
                {
                    var itemOnScene = Instantiate(item.item.Prefab, pos);
                    itemOnScene.Init(item.item, _inventoryController);
                    _itemsQueue.Add(itemOnScene);
                }
            }

            _storageController = Instantiate(_storageData.StorageController, _storagePoint);
            _boneFire = Instantiate(_boneFirePrefab, _boneFirePoint);
        }
    }
}