using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vikings.Building;

namespace Vikings.UI
{
    public class MenuElement : MonoBehaviour
    {
        public int priority;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _upgradeBtn;
        [SerializeField] private Image[] _priceForUpgradeImage;
        [SerializeField] private TMP_Text[] _priceForUpgradeCount;
        [SerializeField] private TMP_Text _buttonDescription;
        [SerializeField] private TMP_Text _requiredText;

        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _defaultSprite;


        public void UpdateUI(string itemName, string description, int level, Sprite icon,
            PriceToUpgrade[] priceToUpgrades, int priority)
        {
            this.priority = priority;
            _name.text = itemName;
            _description.text = description;
            _level.text = level == 0 ? "lvl:1" : $"lvl:{level}";
            _icon.sprite = icon;
            for (int i = 0; i < _priceForUpgradeImage.Length; i++)
            {
                _priceForUpgradeImage[i].sprite = priceToUpgrades[i].itemData.icon;
                _priceForUpgradeCount[i].text = priceToUpgrades[i].count.ToString();
            }
        }

        public void SetButtonDescription(bool isCreate)
        {
            _buttonDescription.text = isCreate ? "Create" : "Upgrade";
        }


        public void SetEnable(bool isEnable, string requiredText)
        {
            if (isEnable)
            {
                _requiredText.gameObject.SetActive(false);
                _upgradeBtn.interactable = true;
                _upgradeBtn.image.sprite = _activeSprite;
            }
            else
            {
                _requiredText.gameObject.SetActive(true);
                _requiredText.text = requiredText;
                _upgradeBtn.interactable = false;
                _upgradeBtn.image.sprite = _defaultSprite;
            }
        }

        public void AddOnClickListener(UnityAction action)
        {
            _upgradeBtn.onClick.AddListener(action);
        }
    }
}