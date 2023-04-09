using System.Linq;
using UnityEngine;
using Vikings.Inventory;

namespace Vikings.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventoryData;
        [SerializeField] private WeaponData _weaponData;

        public float GetCurrentCollectTime()
        {
            return _weaponData.CollectTime;
        }

        public void UpgradeCurrentWeapon()
        {
            var inventory = _inventoryData.GetInventory();
            foreach (var item in _weaponData.weaponUpgradePrices)
            {
                var currentItem = inventory.FirstOrDefault(x => x.itemData.ID == item.itemData.ID);
                if (currentItem == null || currentItem.count < item.count)
                {
                    return;
                }
            }
            
            foreach (var item in _weaponData.weaponUpgradePrices)
            {
                _inventoryData.ChangeItemCount(item.itemData, item.count);
            }
            _weaponData.Upgrade();
        }
    }
}