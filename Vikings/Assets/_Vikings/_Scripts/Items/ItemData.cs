using _Vikings._Scripts.Refactoring;
using Vikings.SaveSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Vikings.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 1)]
    public class ItemData : ScriptableObject, IData, IItemData
    {
        public AnimatorOverrideController AnimatorOverride => _animatorOverride;

        public Sprite icon;
        public string nameText;
        public string description;

        public GameObject view;

        public AudioClip _actionSound;
        public AudioMixerGroup _mixer;


        public bool IsOpen
        {
            get => _isOpen;
            set => _isOpen = value;
        }

        public int ID => _id;

        public string ItemName => _itemName;

        public ItemController Prefab => _prefab;

        [SerializeField] private int _id;

        [SerializeField] private ItemController _prefab;

        [SerializeField] private int _dropCount;

        [SerializeField] private string _itemName;

        [SerializeField] private bool _isOpen;

        [SerializeField] private float _collectTime;

        [SerializeField] private AnimatorOverrideController _animatorOverride;

        [SerializeField] private ResourceType _resourceType;
        

        public int DropCount
        {
            get => _dropCount;
            set => _dropCount = value;
        }

        public int Priority { get; set; }

        public float CollectTime
        {
            get => _collectTime;
            set => _collectTime = value;
        }

        public ResourceType ResourceType => _resourceType;

        public GameObject View => view;


        public void Save()
        {
            // SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            // var data = SaveLoadSystem.LoadData(this) as ItemData;
            // if (data != null)
            // {
            //     _isOpen = data._isOpen;
            // }
        }
    }
}