using System;
using System.Collections;
using UnityEngine;
using Vikings.Inventory;

namespace Vikings.Items
{
    public class ItemController : MonoBehaviour
    {
        public ItemData Item => _itemData;
        public Action<ItemData, int> OnCollect;
        [SerializeField] private GameObject _model;
        
        private const float DELAY_ENABLE = 5f;
        private ItemData _itemData;
        

        public void Init(ItemData itemData, InventoryController inventoryController)
        {
            OnCollect += inventoryController.ChangeItemCount;
            _itemData = itemData;
        }

        public void ActivateItem(float collectTime)
        {
            StartCoroutine(GetItemCoroutine(collectTime));
        }

        private IEnumerator GetItemCoroutine(float collectTime)
        {
            yield return new WaitForSeconds(collectTime);
            _model.SetActive(false);
            OnCollect?.Invoke(_itemData, _itemData.DropCount);
            yield return new WaitForSeconds(DELAY_ENABLE);
            _model.SetActive(true);
        }
    }
}