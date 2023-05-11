using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsManager : MonoBehaviour
{
    [SerializeField] private Button _craftButton;
    [SerializeField] private Button _characterButton;

    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _defaultSprite;
    

    public void EnableButtons(bool isEnable)
    {
        _craftButton.interactable = isEnable;
        _craftButton.image.sprite = isEnable ? _activeSprite : _defaultSprite;
        _characterButton.interactable = isEnable;
        _characterButton.image.sprite = isEnable ? _activeSprite : _defaultSprite;
    }
    
}
