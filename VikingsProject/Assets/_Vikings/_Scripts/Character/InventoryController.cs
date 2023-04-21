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
        
        private IGetItem _currentItem;
        private int _count;

        public void SetItem(IGetItem itemData)
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
                itemData = _currentItem.GetItemData()
            };
            _currentItem = null;
            _count = 0;
            return price;
        }

        private IEnumerator CollectItemsCoroutine(WeaponData weaponData)
        {
            while (_count < _currentItem.GetItemData().DropCount)
            {
                yield return new WaitForSeconds(weaponData.CollectTime);
                _count++;
                yield return null;
            }
            _currentItem.TakeItem();
            OnCollect?.Invoke();
        }
    }
}