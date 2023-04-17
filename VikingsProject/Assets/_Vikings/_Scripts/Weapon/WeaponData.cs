using System;
using System.Collections.Generic;
using UnityEngine;
using Vikings.Building;

namespace Vikings.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData", order = 3)]
    public class WeaponData : ScriptableObject
    {
        public Action<int, float> OnUpgrade;
        public List<PriceToUpgrade> weaponUpgradePrices = new();


        public int ID => _id;
        public int Level => _level;
        public float CollectTime => _collectTime;
        
        [SerializeField] private int _id;
        [SerializeField] private int _level;
        [SerializeField] private float _collectTime;

        public void Upgrade()
        {
            _level++;
            if (_collectTime > 0)
            {
                _collectTime--;
            }
            OnUpgrade?.Invoke(_level, _collectTime);
        }
    }
}