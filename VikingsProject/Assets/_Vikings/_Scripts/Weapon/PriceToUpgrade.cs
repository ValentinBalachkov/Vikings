using System;
using Vikings.Items;

namespace Vikings.Building
{
    [Serializable]
    public class PriceToUpgrade
    {
        public ItemData itemData;
        public int count;
    }
}