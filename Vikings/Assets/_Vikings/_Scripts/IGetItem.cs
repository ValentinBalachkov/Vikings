using UnityEngine;
using Vikings.Items;

namespace Vikings.Building
{
    public interface IGetItem
    {
        bool IsEngaged { get; set; }
        int Priority { get; set; }
        bool DisableToGet { get; set; }
        Transform GetItemPosition();
        void TakeItem();
        ItemData GetItemData();
    }
}