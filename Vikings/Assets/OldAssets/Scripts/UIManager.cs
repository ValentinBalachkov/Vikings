using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject craftScreen;
    [SerializeField] GameObject shopScreen;
    [SerializeField] GameObject upgradeScreen;
    // Start is called before the first frame update
    void Start()
    {
        Messenger.AddListener(GameEvent.OPEN_CRAFTCREEN, OpenCraftScreen);
        Messenger.AddListener(GameEvent.OPEN_SHOPSCREEN, OpenShopScreen);
        Messenger.AddListener(GameEvent.OPEN_UPGRADESCREEN, OpenUpgradeScreen);
        Messenger.AddListener(GameEvent.ENABLE_MAINSCREEN, CloseOtherScreens);
    }

    void OpenCraftScreen()
    {
        craftScreen.SetActive(true);
    }
    void OpenShopScreen()
    {
        shopScreen.SetActive(true);
    }
    void OpenUpgradeScreen()
    {
        upgradeScreen.SetActive(true);
    }

    void CloseOtherScreens()
    {
        upgradeScreen.SetActive(false);
        shopScreen.SetActive(false);
        craftScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
