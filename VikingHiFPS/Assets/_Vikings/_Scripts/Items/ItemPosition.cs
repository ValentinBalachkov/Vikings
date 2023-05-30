using System;
using UnityEngine;

namespace Vikings.Items
{
    [Serializable]
    public class ItemPosition
    {
        public ItemLevelPosition[] position;
        public ItemData item;
    }
    
    [Serializable]
    public class ItemLevelPosition
    {
        public Transform[] position;
        public int level;
    }
}