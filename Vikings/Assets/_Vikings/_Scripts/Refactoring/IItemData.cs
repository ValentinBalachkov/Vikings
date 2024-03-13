using UnityEngine;

namespace _Vikings._Scripts.Refactoring
{
    public interface IItemData
    {
        public int DropCount { get; set; }
        public int Priority { get; set; }
        public float CollectTime { get; set; }
        public ResourceType ResourceType { get;}

        public GameObject View { get;}
    }
}