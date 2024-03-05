using TMPro;
using UnityEngine;

namespace Vikings.Building
{
    public class StorageView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private TMP_Text _levelText;
       // [SerializeField] private StorageController _storageController;

        private void Start()
        {
            //_storageController.OnUpgradeStorage += UpdateUIUpgrade;
        }
        private void UpdateUIUpgrade(int maxCount, int level)
        {
            _levelText.text = $"level: {level} \n max: {maxCount}";
        }
    }
}