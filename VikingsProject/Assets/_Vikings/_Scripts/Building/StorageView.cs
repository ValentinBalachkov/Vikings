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
            _storageData.OnChangeCountStorage += UpdateUICount;
            _storageData.OnUpgradeStorage += UpdateUIUpgrade;
        }
        private void UpdateUICount(int itemCount, int maxCount)
        {
            _countText.text = $"{itemCount}/{maxCount}";
        }
        
        private void UpdateUIUpgrade(int maxCount, int level)
        {
            _levelText.text = $"level: {level} \n max: {maxCount}";
        }
    }
}