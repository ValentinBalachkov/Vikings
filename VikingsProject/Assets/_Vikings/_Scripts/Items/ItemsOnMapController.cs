using System.Collections.Generic;
using UnityEngine;
using Vikings.Inventory;

namespace Vikings.Items
{
    public class ItemsOnMapController : MonoBehaviour
    {
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private ItemPosition[] _itemPositions;
        private Queue<ItemController> _itemsQueue = new();

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
        }
    }
}