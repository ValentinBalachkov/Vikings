using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OfflineFarmView : MonoBehaviour
{
    [SerializeField] private OfflineIconsData[] _icons;
    [SerializeField] private GameObject _buildingPanel;
    [SerializeField] private GameObject _resourcesPanel;
    [SerializeField] private TMP_Text _buildingDescriptionText;
    
    

    public void OpenWindow(Dictionary<int, int> itemsDict, string craftName, int level)
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
            _buildingDescriptionText.text = $"{craftName} lvl {level}";
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