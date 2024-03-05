using PanelManager.Scripts.Panels;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsManager : ViewBase
{
    public override PanelType PanelType => PanelType.Overlay;
    public override bool RememberInHistory => false;
    
    [SerializeField] private Button _craftButton;


    public void EnableButtons(bool isEnable)
    {
        _craftButton.interactable = isEnable;
    }
    
}
