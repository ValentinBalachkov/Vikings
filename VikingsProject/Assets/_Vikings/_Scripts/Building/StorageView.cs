using TMPro;
using UnityEngine;

namespace Vikings.Building
{
    public class StorageView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private StorageData _storageData;

        private void Start()
        {
            _storageData.OnChangeCountStorage += UpdateUI;
            _storageData.OnUpgradeStorage += UpdateUI;
        }
        private void UpdateUI(int itemCount)
        {
            _countText.text = $"{_storageData.ItemType.ItemName}: {itemCount}";
        }
        
        private void UpdateUI(int maxCount, int level)
        {
            _levelText.text = $"level: {level} \n max: {maxCount}";
        }
    }
}