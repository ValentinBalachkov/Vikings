using System.Linq;
using UnityEngine;
using Vikings.Inventory;

namespace Vikings.Building
{
    public class StorageController : MonoBehaviour
    {
        [SerializeField] private StorageData _storageData;
        [SerializeField] private InventoryData _inventoryData;

        public void SetItemsOnStorage()
        {
           var item = _inventoryData.GetInventory().FirstOrDefault(x => x.itemData.ID == _storageData.ItemType.ID);
           if (item == null)
           {
               return;
           }
           
           if (item.count + _storageData.Count < _storageData.MaxStorageCount)
           {
               _storageData.ChangeStorageCount(item.count);
               _inventoryData.ChangeItemCount(item.itemData, -item.count);
           }
           else
           {
               int countToChange = _storageData.MaxStorageCount - _storageData.Count;
               _storageData.ChangeStorageCount(countToChange);
               _inventoryData.ChangeItemCount(item.itemData, -countToChange);
           }
        }

        public void UpgradeStorage()
        {
            _storageData.UpgradeStorage();
        }
        
    }
}