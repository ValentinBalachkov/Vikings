using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsManager : MonoBehaviour
{
    [SerializeField] private Button _craftButton;
    [SerializeField] private Button _characterButton;


    public void EnableButtons(bool isEnable)
    {
        _craftButton.interactable = isEnable;
        _characterButton.interactable = isEnable;
    }
    
}
