using System;
using System.Collections;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class InventoryController : MonoBehaviour
    {
        public Action OnCollect;
        
        private ItemController _currentItem;
        private int _count;

        public void SetItem(ItemController itemData)
        {
            _currentItem = itemData;
        }

        public void CollectItem(WeaponData weaponData)
        {
            StartCoroutine(CollectItemsCoroutine(weaponData));
        }

        public PriceToUpgrade SetItemToStorage()
        {
            Debug.Log(_currentItem);
            var price = new PriceToUpgrade
            {
                count = _count,
                itemData = _currentItem.Item
            };
            _currentItem = null;
            _count = 0;
            return price;
        }

        private IEnumerator CollectItemsCoroutine(WeaponData weaponData)
        {
            while (_count < _currentItem.Item.DropCount)
            {
                yield return new WaitForSeconds(weaponData.CollectTime);
                _count++;
                yield return null;
            }
            _currentItem.DisableItemOnMap();
            OnCollect?.Invoke();
        }
    }
}