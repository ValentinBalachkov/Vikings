using System.Collections;
using UnityEngine;
using Vikings.Items;

namespace Vikings.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventoryData;
        private ItemData[] _itemDatas;

        public void ChangeItemCount(ItemData itemData, int count)
        {
            _inventoryData.ChangeItemCount(itemData, count);
        }

        private void Awake()
        {
            _itemDatas = Resources.LoadAll<ItemData>($"ItemData");
        }
    }
}