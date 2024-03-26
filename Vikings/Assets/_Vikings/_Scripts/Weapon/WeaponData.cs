using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;
using ItemCount = _Vikings.Refactoring.Character.ItemCount;


    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData", order = 3)]
    public class WeaponData : ScriptableObject
    {
        public string saveKey;
        public Sprite icon;
        public Sprite iconOfflineFarm;
        public string nameText;
        public string description;
        public string required;
        public Sprite requiredSprite;
        public int priority;
        public TaskData taskData;
        public List<ItemCount> priceToBuy = new();
        public List<ItemData> avaleableResources = new();
        public int craftingTime;
        
    }
