using System;
using System.Collections;
using UnityEngine;
using Vikings.Building;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class InventoryController : MonoBehaviour
    {
        public Action OnCollect;
        [SerializeField] private CharactersConfig _charactersConfig;

        private IGetItem _currentItem;
        private int _count;

        public void SetItem(IGetItem itemData)
        {
            _currentItem = itemData;
        }

        public void CollectItem()
        {
            StartCoroutine(CollectItemsCoroutine());
        }

        public PriceToUpgrade SetItemToStorage()
        {
            Debug.Log(_currentItem);
            var price = new PriceToUpgrade
            {
                count = _count,
                itemData = _currentItem.GetItemData()
            };
            _currentItem = null;
            _count = 0;
            return price;
        }

        private IEnumerator CollectItemsCoroutine()
        {
            var item = _currentItem.GetItemData();
            yield return new WaitForSeconds(item.CollectTime);
            _count = _charactersConfig.ItemsCount < item.DropCount ? _charactersConfig.ItemsCount : item.DropCount;
            _currentItem.TakeItem();
            OnCollect?.Invoke();
        }
    }
}