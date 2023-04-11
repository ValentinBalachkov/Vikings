using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Items;

namespace Vikings.Inventory
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Data/InventoryData", order = 2)]
    public class InventoryData : ScriptableObject
    {
        public Action<ItemData> OnInventoryChange;
        
        [SerializeField] private List<ItemsCountData>  _itemsOnInventory;

        public List<ItemsCountData> GetInventory()
        {
            return _itemsOnInventory;
        }

        public void ChangeItemCount(ItemData itemData, int count)
        {
            var item = _itemsOnInventory.FirstOrDefault(x => x.itemData.ID == itemData.ID);
            if (item != null)
            {
                if (item.itemData.LimitCount <= item.count + count)
                {
                    item.count = item.itemData.LimitCount;
                }
                else
                {
                    item.count += count;
                }
            }
            else
            {
                _itemsOnInventory.Add(new ItemsCountData()
                {
                    count = count,
                    itemData = itemData
                });
            }
            
            OnInventoryChange?.Invoke(itemData);
        }

        public bool IsFullInventory()
        {
            var list = _itemsOnInventory.Where(x => x.count < x.itemData.LimitCount).ToList();
            return list.Count == 0;
        }
    
    }
}

