using System;
using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using PanelManager.Scripts.Panels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OfflineFarmView : ViewBase
{
    public override PanelType PanelType => PanelType.Screen;
    public override bool RememberInHistory => false;

    [SerializeField] private Button _closeButton;
    
    
    [SerializeField] private OfflineIconsData[] _icons;
    [SerializeField] private GameObject _buildingPanel;
    [SerializeField] private GameObject _resourcesPanel;
    [SerializeField] private TMP_Text _buildingLevelText;
    [SerializeField] private Image _buildingIcon;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _closeButton.OnClickAsObservable().Subscribe(_ =>
        {
            _panelManager.PlaySound(UISoundType.Close);
            gameObject.SetActive(false);
        }).AddTo(_panelManager.Disposable);
    }

    public void OpenWindow(Dictionary<ResourceType, int> itemsDict, int level, Sprite sprite = null)
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
                var icon = _icons.FirstOrDefault(x => x.resourceType == item.Key);
                DebugLogger.SendMessage(item.Value.ToString(), Color.blue);
                icon.text.text = item.Value.ToString();
                icon.icon.SetActive(true);
            }
            
            _resourcesPanel.SetActive(true);
        }

        if (level != 0)
        {
            _buildingIcon.sprite = sprite;
            _buildingLevelText.text = level.ToString();
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
    public ResourceType resourceType;
}
