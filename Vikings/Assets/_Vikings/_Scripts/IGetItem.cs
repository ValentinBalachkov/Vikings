using UnityEngine;
using Vikings.Items;

namespace Vikings.Building
{
    public interface IGetItem
    {
        public int Priority { get; set; }
        public bool DisableToGet { get; set; }
        public Transform GetItemPosition();

        public void TakeItem();

        public ItemData GetItemData();
    }
}