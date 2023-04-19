using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private TMP_Text[] _itemsCountText;

        private List<StorageController> _storageControllers = new();

        public void AddStorageController(StorageController storageController)
        {
            storageController.OnChangeCountStorage += UpdateUI;
            _storageControllers.Add(storageController);
        }

        private void UpdateUI(ItemData itemData)
        {
            for (int i = 0; i < _storageControllers.Count; i++)
            {
                var buildingData = _storageControllers[i].BuildingData;
                _itemsCountText[i].text =
                    $"{buildingData.StorageData.ItemType.ItemName}: {buildingData.StorageData.Count}/{buildingData.StorageData.MaxStorageCount}";
            }
        }
    }
}