using System;
using PanelManager.Scripts;
using UnityEngine;

public class MainPanelManager : PanelManagerBase
{
    public void Init()
    {
        CreatePanelsFromSettings();
    }

    private void OnDestroy()
    {
        Disposable.Dispose();
    }
    
}

public enum UISoundType
{
    Open,
    Close,
    UpgradePeople,
    CreateBuilding
}

[Serializable]
public class UISoundData
{
    public UISoundType type;
    public AudioSource audio;
}