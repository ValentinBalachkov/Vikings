using UnityEngine;

namespace Vikings.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 1)]
    public class ItemData : ScriptableObject
    {
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                if (_isOpen)
                {
                    SaveData();
                }
            } 
        }

        public int ID => _id;

        public string ItemName => _itemName;

        public int DropCount => _dropCount;

        public Sprite Icon => _icon;

        public ItemController Prefab => _prefab;

        [SerializeField] private int _id;

        [SerializeField] private Sprite _icon;

        [SerializeField] private ItemController _prefab;

        [SerializeField] private int _dropCount;

        [SerializeField] private string _itemName;

        [SerializeField] private bool _isOpen;

        public void LoadData()
        {
            if (!PlayerPrefs.HasKey(_itemName)) return;
            var value = PlayerPrefs.GetInt(_itemName);
            if (value == 1)
            {
                _isOpen = true;
            }
        }

        private void SaveData()
        {
            int value = _isOpen ? 1 : 0;
            PlayerPrefs.SetInt(_itemName, value);
        }
    }
}