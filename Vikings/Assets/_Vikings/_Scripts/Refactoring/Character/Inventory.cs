using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using Vikings.Weapon;

namespace _Vikings.Refactoring.Character
{
    public class Inventory
    {
        private List<WeaponData> _weapons = new();
        private ItemCount _itemCount;

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
            if (_itemCount == null) { return null; }
            
            var item =  new ItemCount
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