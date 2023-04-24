using System;
using TMPro;
using UnityEngine;
using Vikings.Building;

namespace Vikings.UI
{
    public class CraftingTableView : MonoBehaviour
    {
        [SerializeField] private CraftingTableController _craftingTableController;
        [SerializeField] private TMP_Text _count;

        private void Awake()
        {
            _craftingTableController.OnChangeCount += UpdateUI;
        }

        private void UpdateUI(PriceToUpgrade[] priceList, PriceToUpgrade[] currentItems)
        {
            _count.text = "";

            for (int i = 0; i < priceList.Length; i++)
            {
                _count.text += $"{priceList[i].itemData.ItemName}: {currentItems[i].count}/{priceList[i].count}\n";
            }
        }
        
    
    }
}

