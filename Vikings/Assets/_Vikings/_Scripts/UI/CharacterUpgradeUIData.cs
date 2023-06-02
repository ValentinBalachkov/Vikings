using UnityEngine;

namespace Vikings.UI
{
    [CreateAssetMenu(fileName = "CharacterUpgradeUIData", menuName = "Data/CharacterUpgradeUIData", order = 10)]
    public class CharacterUpgradeUIData : ScriptableObject
    {
        public Sprite icon;
        public string upgradeName;
        public string description;
        public UpgradeCharacterEnum upgradeCharacterEnum;
    }
}