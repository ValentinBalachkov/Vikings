using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsManager : MonoBehaviour
{
    [SerializeField] private Button _craftButton;


    public void EnableButtons(bool isEnable)
    {
        _craftButton.interactable = isEnable;
    }
    
}
