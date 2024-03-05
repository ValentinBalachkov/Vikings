using UnityEngine;
using Vikings.Object;

namespace _Vikings._Scripts.Refactoring
{
    public abstract class AbstractResource : AbstractObject
    {
        public override Transform GetPosition()
        {
            return transform;
        }

        public abstract IItemData GetItemData();

        public abstract void SetItemData(IItemData itemData);

        public abstract bool IsEnableToGet();
    }
}