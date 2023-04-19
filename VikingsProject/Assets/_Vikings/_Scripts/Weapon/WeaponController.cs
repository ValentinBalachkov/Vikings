using System.Linq;
using UnityEngine;
using Vikings.Building;

namespace Vikings.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponData WeaponData => _weaponData;
        
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private StorageOnMap _storageOnMap;
        
        public void UpgradeCurrentWeapon()
        {
            if (_weaponData.Level == 3)
            {
                return;
            }
            
            foreach (var weaponPrice in _weaponData.weaponUpgradePrices)
            {
                var storage = _storageOnMap.StorageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == weaponPrice.itemData.ID);
                if (storage != null && weaponPrice.count <= storage.BuildingData.StorageData.Count)
                {
                    continue;
                }

                return;
            }

            foreach (var item in _weaponData.weaponUpgradePrices)
            {
                var storage = _storageOnMap.StorageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == item.itemData.ID);
                if (storage != null) storage.ChangeStorageCount(new PriceToUpgrade{count = -item.count, itemData = item.itemData});
            }
            _weaponData.Upgrade();
        }
    }
}