using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vikings.Building;

namespace Vikings.UI
{
    public class CraftingTableView : MonoBehaviour
    {
        [SerializeField] private CraftingTableController _craftingTableController;
        [SerializeField] private TMP_Text _count;
        [SerializeField] private Image _progressBar;
        

        private void Awake()
        {
            _craftingTableController.OnChangeCount += UpdateUI;
        }

        public void UpgradeProgressBar()
        {
            
        }

        private void UpdateUI(ItemCount[] priceList, ItemCount[] currentItems)
        {
            _count.text = "";

            for (int i = 0; i < priceList.Length; i++)
            {
                _count.text += $"{priceList[i].itemData.ItemName}: {currentItems[i].count}/{priceList[i].count}\n";
            }
        }
        
    
    }
}

