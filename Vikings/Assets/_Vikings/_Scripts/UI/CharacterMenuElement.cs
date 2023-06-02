using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Vikings.UI
{
    public class CharacterMenuElement : MonoBehaviour
    {
        public UpgradeCharacterEnum upgradeCharacterEnum;
        
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private Button _upgradeBtn;

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Image _iconImage;
        
        public void AddBtnListener(UnityAction action)
        {
            _upgradeBtn.onClick.AddListener(action);
        }

        public void Init(CharacterUpgradeUIData data)
        {
            _name.text = data.upgradeName;
            _description.text = data.description;
            _iconImage.sprite = data.icon;
            upgradeCharacterEnum = data.upgradeCharacterEnum;
        }

        public void UpdateUI(int level, int cost, bool isEnable)
        {
            _levelText.text = $"lvl:{level}";
            _costText.text = cost.ToString();
            _upgradeBtn.interactable = isEnable;
        }
        
        
    }
}