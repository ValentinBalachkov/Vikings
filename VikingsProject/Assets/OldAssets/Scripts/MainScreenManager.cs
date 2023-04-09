using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField] GameObject shopButton;
    [SerializeField] GameObject craftButton;
    [SerializeField] GameObject upgradeButton;
    [SerializeField] GameObject craftResourcePopup;
    [SerializeField] GameObject craftPopup;
    [SerializeField] TMP_Text woodText;
    [SerializeField] TMP_Text stoneText;
    [SerializeField] TMP_Text foodText;
    [SerializeField] TMP_Text craftStoneText;
    [SerializeField] TMP_Text craftWoodText;
    [SerializeField] TMP_Text BuildingProcessText;
    [SerializeField] Vector3 offset = new Vector3 (0, 100, 0);

    CraftManager craftManager;
    Camera mainCamera;
    bool isEnabled = true;
    int woodStorage;
    int stoneStorage;
    int requiredWood;
    int requiredStone;
    int curWood;
    int curStone;
    Vector3 objectiveScreenPos;

    private void Awake() 
    {
        Messenger<(string buildingId, int requiredWoodMessage, int requiredStoneMessage)>.AddListener(GameEvent.START_CRAFT, OnStartCraft);
        Messenger<(string resourceType, string taskType, int amount, GameObject character)>.AddListener(GameEvent.FINISH_TASK, OnFinishTask);
        Messenger<float>.AddListener(GameEvent.SET_BUILDING_PROGRESS, SetBuildingProgress);
        Messenger<(string resourceType, int amount)>.AddListener(GameEvent.PICK_FROM_STORAGE, OnPickStorage);
    }
    // Start is called before the first frame update
    void Start()
    {
        craftManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CraftManager>();
        Messenger.AddListener(GameEvent.ENABLE_MAINSCREEN, EnableMainScreen);
        Messenger.AddListener(GameEvent.DISABLE_MAINSCREEN, DisableMainScreen);

        stoneText.text = string.Format("{0}/{1}", 0, craftManager.maxStoneStorage);
        foodText.text = string.Format("{0}/{1}", 0, craftManager.maxFoodStorage);
        woodText.text = string.Format("{0}/{1}", 0, craftManager.maxWoodStorage);

        shopButton.GetComponent<Button>().onClick.AddListener(openShopScreen);
        craftButton.GetComponent<Button>().onClick.AddListener(openCraftScreen);
        upgradeButton.GetComponent<Button>().onClick.AddListener(openUpgradeScreen);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void OnPickStorage((string resourceType, int amount) message)
    {
        if (message.resourceType == "WoodStorage")
        {
            woodStorage -= message.amount;
            woodText.text = string.Format("{0}/{1}", woodStorage, craftManager.maxWoodStorage);
        }
        else if (message.resourceType == "StoneStorage")
        {
            stoneStorage -= message.amount;
            stoneText.text = string.Format("{0}/{1}", stoneStorage, craftManager.maxStoneStorage);
        }
    }

    void OnStartCraft((string buildingId, int requiredWoodMessage, int requiredStoneMessage) message)
    {
        var objective = craftManager.buildingsDict[message.buildingId];
        objectiveScreenPos = mainCamera.WorldToScreenPoint(objective.transform.position);
        craftResourcePopup.SetActive(true);
        craftResourcePopup.transform.position = objectiveScreenPos + offset;
        requiredWood = message.requiredWoodMessage;
        requiredStone = message.requiredStoneMessage;
        craftWoodText.text = string.Format("{0}/{1}", 0, requiredWood);
        craftStoneText.text = string.Format("{0}/{1}", 0, requiredStone);
    }

    void OnFinishTask((string resourceType, string taskType, int amount, GameObject character) message)
    {
        if (message.taskType == "CraftCollection")
            ProcessCraftMessage(message);
        else if (message.taskType == "StorageCollection")
            ProcessCollectMessage(message);
    }

    void ProcessCollectMessage((string resourceType, string taskType, int amount, GameObject character) message)
    {
        if (message.resourceType == "Wood")
        {
            woodStorage += message.amount;
            woodText.text = string.Format("{0}/{1}", woodStorage, craftManager.maxWoodStorage);
        }
        else if (message.resourceType == "Stone")
        {
            stoneStorage += message.amount;
            stoneText.text = string.Format("{0}/{1}", stoneStorage, craftManager.maxStoneStorage);
        }
    }
    void ProcessCraftMessage((string resourceType, string taskType, int amount, GameObject character) message)
    {
        if (message.resourceType == "WoodStorage")
        {
            curWood += message.amount;
            craftWoodText.text = string.Format("{0}/{1}", curWood, requiredWood);
        }
        else if (message.resourceType == "Wood")
        {
            curWood += message.amount;
            craftWoodText.text = string.Format("{0}/{1}", curWood, requiredWood);
        }
        else if (message.resourceType == "Stone")
        {
            curStone += message.amount;
            craftStoneText.text = string.Format("{0}/{1}", curStone, requiredStone);
        }
        else if (message.resourceType == "StoneStorage")
        {
            curStone += message.amount;
            craftStoneText.text = string.Format("{0}/{1}", curStone, requiredStone);
        }
        CheckBuildingStart();
    }

    void CheckBuildingStart()
    {
        if (curStone == requiredStone && curWood == requiredWood)
        {
            craftPopup.SetActive(true);
            craftResourcePopup.SetActive(false);
            craftPopup.transform.position = objectiveScreenPos + offset;
        }
    }

    void SetBuildingProgress(float buildingProgress)
    {
        int buildingProgressInt = (int)buildingProgress;
        BuildingProcessText.text = string.Format("{0}/{1}", buildingProgressInt, craftManager.buildingMaxProgress);
        if (buildingProgressInt >= craftManager.buildingMaxProgress)
            craftPopup.SetActive(false);
    }

    void EnableMainScreen()
    {
        isEnabled = true;
    }
    void DisableMainScreen()
    {
        isEnabled = false;
    }

    void openShopScreen()
    {   
        if (isEnabled)
        {
            Messenger.Broadcast(GameEvent.OPEN_SHOPSCREEN);
            isEnabled = false;
        }
    }

    void openCraftScreen()
    {
        if (isEnabled)
        {
            Messenger.Broadcast(GameEvent.OPEN_CRAFTCREEN);
            isEnabled = false;
        }
    }
    void openUpgradeScreen()
    {
        if (isEnabled)
        {
            Messenger.Broadcast(GameEvent.OPEN_UPGRADESCREEN);
            isEnabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
