using System;
using System.Collections.Generic;
using System.Linq;
using PanelManager.Scripts.Interfaces;
using PanelManager.Scripts.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineFarmView : ViewBase, IAcceptArg<OfflineFarmViewArgs>
{
    public override PanelType PanelType => PanelType.Screen;
    public override bool RememberInHistory => false;
    
    [SerializeField] private OfflineIconsData[] _icons;
    [SerializeField] private GameObject _buildingPanel;
    [SerializeField] private GameObject _resourcesPanel;
    [SerializeField] private TMP_Text _buildingNameText;
    [SerializeField] private TMP_Text _buildingLevelText;
    [SerializeField] private Image _buildingIcon;

    public void OpenWindow(Dictionary<int, int> itemsDict, string craftName, int level, Sprite sprite = null)
    {
        DebugLogger.SendMessage($"{itemsDict.Count}, {level}", Color.cyan);
        
        if (itemsDict.Count == 0 && level == 0)
        {
            return;
        }
        
        foreach (var item in _icons)
        {
            item.icon.SetActive(false);
            _buildingPanel.SetActive(false);
        }

        if (itemsDict.Count == 0)
        {
            _resourcesPanel.SetActive(false);
        }
        else
        {
            foreach (var item in itemsDict)
            {
                var icon = _icons.FirstOrDefault(x => x.itemId == item.Key);
                icon.text.text = item.Value.ToString();
                icon.icon.SetActive(true);
            }
            
            _resourcesPanel.SetActive(true);
        }

        if (level != 0)
        {
            _buildingIcon.sprite = sprite;
            _buildingNameText.text = $"{craftName}";
            _buildingLevelText.text = $"lvl:{level}";
            _buildingPanel.SetActive(true);
        }
        else
        {
            _buildingPanel.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public void AcceptArg(OfflineFarmViewArgs arg)
    {
        DebugLogger.SendMessage($"{arg.itemsDict.Count}, {arg.level}", Color.cyan);
        
        if (arg.itemsDict.Count == 0 && arg.level == 0)
        {
            return;
        }
        
        foreach (var item in _icons)
        {
            item.icon.SetActive(false);
            _buildingPanel.SetActive(false);
        }

        if (arg.itemsDict.Count == 0)
        {
            _resourcesPanel.SetActive(false);
        }
        else
        {
            foreach (var item in arg.itemsDict)
            {
                var icon = _icons.FirstOrDefault(x => x.itemId == item.Key);
                icon.text.text = item.Value.ToString();
                icon.icon.SetActive(true);
            }
            
            _resourcesPanel.SetActive(true);
        }

        if (arg.level != 0)
        {
            _buildingIcon.sprite = arg.sprite;
            _buildingNameText.text = $"{arg.craftName}";
            _buildingLevelText.text = $"lvl:{arg.level}";
            _buildingPanel.SetActive(true);
        }
        else
        {
            _buildingPanel.SetActive(false);
        }
        gameObject.SetActive(true);
    }
}

[Serializable]
public class OfflineIconsData
{
    public GameObject icon;
    public TMP_Text text;
    public int itemId;
}

[Serializable]
public class OfflineFarmViewArgs
{
    public Dictionary<int, int> itemsDict;
    public string craftName;
    public int level;
    public Sprite sprite;
}