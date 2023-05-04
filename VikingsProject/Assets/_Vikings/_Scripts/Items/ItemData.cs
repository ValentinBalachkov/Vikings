using SecondChanceSystem.SaveSystem;
using UnityEngine;

namespace Vikings.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 1)]
    public class ItemData : ScriptableObject, IData
    {
        
        public Sprite icon;
        public string nameText;
        public string description;
        
        public bool IsOpen
        {
            get => _isOpen;
            set => _isOpen = value;
        }

        public int ID => _id;

        public string ItemName => _itemName;

        public int DropCount => _dropCount;
        
        public ItemController Prefab => _prefab;

        [SerializeField] private int _id;

        [SerializeField] private ItemController _prefab;

        [SerializeField] private int _dropCount;

        [SerializeField] private string _itemName;

        [SerializeField] private bool _isOpen;

        public void Save()
        {
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as ItemData;
            if (data != null)
            {
                _isOpen = data._isOpen;
            }
        }
    }
}