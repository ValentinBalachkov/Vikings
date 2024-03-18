using _Vikings._Scripts.Refactoring;
using Vikings.SaveSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Vikings.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 1)]
    public class ItemData : ScriptableObject, IItemData
    {
        public AnimatorOverrideController AnimatorOverride => _animatorOverride;

        public Sprite icon;
        public string nameText;
        public string description;

        public GameObject view;

        public AudioClip _actionSound;
        public AudioMixerGroup _mixer;

        public int disableDelaySecond;
        public int ID => _id;

        public string ItemName => _itemName;
        

        [SerializeField] private int _id;

        [SerializeField] private int _dropCount;

        [SerializeField] private string _itemName;

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
        
    }
}