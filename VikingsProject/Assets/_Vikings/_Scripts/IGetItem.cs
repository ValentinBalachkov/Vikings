using UnityEngine;
using Vikings.Items;

namespace Vikings.Building
{
    public interface IGetItem
    {
        public Transform GetItemPosition();

        public void TakeItem();

        public ItemData GetItemData();
    }
}