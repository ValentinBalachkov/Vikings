using System.Linq;
using _Vikings._Scripts.Refactoring;
using Vikings.Items;

namespace _Vikings.Refactoring.Character
{
    public class Inventory
    {
        private ItemCount _itemCount;

        private WeaponFactory _weaponFactory;

        public Inventory(WeaponFactory weaponFactory)
        {
            _weaponFactory = weaponFactory;
        }

        public int GetItemPerActionCount(ItemData itemData)
        {
            var weapon = _weaponFactory.GetOpenWeapons().FirstOrDefault(x => x.GetWeaponData().avaleableResources.Contains(itemData));
            return weapon.Level.Value;
        }

        public void SetItemToInventory(ResourceType resourceType, int count)
        {
            _itemCount = new ItemCount
            {
                resourceType = resourceType,
                count = count
            };
        }

        public ItemCount GetItemFromInventory()
        {
            if (_itemCount == null)
            {
                return null;
            }

            var item = new ItemCount
            {
                resourceType = _itemCount.resourceType,
                count = _itemCount.count
            };
            _itemCount = null;
            return item;
        }

        public int CheckItemCount()
        {
            if (_itemCount == null) return 0;
            return _itemCount.count;
        }
    }
}