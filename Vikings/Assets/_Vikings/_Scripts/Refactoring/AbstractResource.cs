using System;
using UnityEngine;
using Vikings.Items;
using Vikings.Object;

namespace _Vikings._Scripts.Refactoring
{
    public abstract class AbstractResource : AbstractObject
    {
        public Action ResourceEnable;
        
        public bool IsTarget;
        public override Transform GetPosition()
        {
            return transform;
        }

        public abstract ItemData GetItemData();

        public abstract void SetItemData(ItemData itemData);

        public abstract bool IsEnableToGet();

        public abstract void ResetState();
    }
}