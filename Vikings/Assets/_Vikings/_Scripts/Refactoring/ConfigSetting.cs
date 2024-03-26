using System.Collections.Generic;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Items;

namespace _Vikings._Scripts.Refactoring
{
    [CreateAssetMenu(fileName = "ConfigSetting", menuName = "Data/ConfigSetting")]
    public class ConfigSetting : ScriptableObject
    {
        public List<BuildingData> buildingsData = new();
        public BuildingData craftingTable;
        public List<WeaponData> weaponsData = new();
        public List<ItemData> resourcesData = new();
        public List<TaskData> tasksData = new();
        public CharactersConfig charactersConfig;
    }
}