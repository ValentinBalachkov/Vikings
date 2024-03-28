using System;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using PanelManager.Scripts.Panels;
using TMPro;
using UnityEngine;

namespace Vikings.UI
{
    public class InventoryView : ViewBase
    {
        public override PanelType PanelType => PanelType.Overlay;
        public override bool RememberInHistory => false;
        
        [SerializeField] private InventoryViewTextData[] _itemsCountText;

        public void SetActiveResourcePanel(ResourceType resourceType, bool status)
        {
            var view = _itemsCountText.FirstOrDefault(x => x.type == resourceType);
            view.panel.SetActive(status);
        }

        public void UpdateUI(int value, int maxCount, ResourceType resourceType)
        {
            var view = _itemsCountText.FirstOrDefault(x => x.type == resourceType);
            view.countText.text = $"{value}/{maxCount}";
        }
    }

    [Serializable]
    public class InventoryViewTextData
    {
        public TMP_Text countText;
        public ResourceType type;
        public GameObject panel;
    }
}