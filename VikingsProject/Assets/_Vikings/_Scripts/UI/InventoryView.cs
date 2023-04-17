using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private StorageOnMap _storageOnMap;
        [SerializeField] private TMP_Text[] _itemsCountText;

        private void Awake()
        {
            foreach (var storage in _storageOnMap.StorageControllers)
            {
                if (storage is StorageController controller)
                    controller.OnChangeCountStorage += UpdateUI;
            }
        }

        private void UpdateUI(ItemData itemData)
        {
            for (int i = 0; i < _storageOnMap.StorageControllers.Count; i++)
            {
                var storageData = _storageOnMap.StorageControllers[i].StorageData;
                _itemsCountText[i].text =
                    $"{storageData.ItemType.ItemName}: {storageData.Count}/{storageData.MaxStorageCount}";
            }
        }
    }
}