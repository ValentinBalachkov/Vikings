using System;
using UnityEngine;

namespace Vikings.Items
{
    [Serializable]
    public class ItemPosition
    {
        public Transform[] position;
        public ItemData item;
    }
}