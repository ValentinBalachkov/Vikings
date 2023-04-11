using UnityEngine;

namespace Vikings.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 1)]
    public class ItemData : ScriptableObject
    {
        public int ID => _id;
        public string ItemName => _itemName;
        public int DropCount => _dropCount;
        public Sprite Icon => _icon;
        public ItemController Prefab => _prefab;
        public int LimitCount => _limitCount;

        [SerializeField] private int _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemController _prefab;
        [SerializeField] private int _dropCount;
        [SerializeField] private string _itemName;
        [SerializeField] private int _limitCount;
    }
}