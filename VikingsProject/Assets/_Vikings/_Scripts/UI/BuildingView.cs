using TMPro;
using UnityEngine;
using Vikings.Building;

namespace Vikings.UI
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourceText;
        [SerializeField] private BuildingController _buildingController;

        private void Awake()
        {
            _buildingController.OnChangeCount += UpdateUI;
        }

        private void UpdateUI(BuildingData buildingData)
        {
            _resourceText.text = "";

            for (int i = 0; i < buildingData.PriceToUpgrades.Length; i++)
            {
                _resourceText.text +=
                    $"{buildingData.currentItemsCount[i].itemData.ItemName}: {buildingData.currentItemsCount[i].count}/{buildingData.PriceToUpgrades[i].count} \n";
            }
        }
        
    }
}