using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private InventoryViewTextData[] _itemsCountText;

        private List<StorageController> _storageControllers = new();

        public void AddStorageController(StorageController storageController)
        {
            storageController.OnChangeCountStorage += UpdateUI;
            _storageControllers.Add(storageController);
        }

        private void UpdateUI(ItemData itemData)
        {
            var storage =
                _storageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == itemData.ID);
            if(storage == null) return;

            var item = _itemsCountText.FirstOrDefault(x => x.item.ID == itemData.ID);
            
            item.countText.text = $" {storage.BuildingData.StorageData.Count}/{storage.BuildingData.StorageData.MaxStorageCount}";
        }
    }

    [Serializable]
    public class InventoryViewTextData
    {
        public TMP_Text countText;
        public ItemData item;
    }
}