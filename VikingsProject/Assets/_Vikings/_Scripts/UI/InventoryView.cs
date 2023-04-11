using TMPro;
using UnityEngine;
using Vikings.Inventory;
using Vikings.Items;

namespace Vikings.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventoryData;
        [SerializeField] private TMP_Text[] _itemsCountText;

        private void Awake()
        {
            _inventoryData.OnInventoryChange += UpdateUI;
        }

        private void UpdateUI(ItemData itemData)
        {
            var inventory = _inventoryData.GetInventory();
            for (int i = 0; i < inventory.Count; i++)
            {
                _itemsCountText[i].text =  $"{inventory[i].itemData.ItemName}: {inventory[i].count}/{inventory[i].itemData.LimitCount}";
            }
        }
    }
}