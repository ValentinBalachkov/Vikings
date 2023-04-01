using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using TMPro;

public class CraftManager : MonoBehaviour
{
    [Serializable]
    public struct Building {
        public string name;
        public Transform transform;
    }
    [SerializeField] Building[] buildings;
    [SerializeField] List<GameObject> characters;

    [SerializeField] TMP_Text woodText;
    [SerializeField] TMP_Text stoneText;
    [SerializeField] TMP_Text foodText;

    public enum ManagerStatus
    {
        Waiting,
        Collecting,
        Crafting
    }

    public ManagerStatus status;
    Transform curBuilding;
    [SerializeField] int characterVolume = 4;
    [SerializeField] int craftSpeed = 10;
    public int maxWoodStorage = 20;
    public int maxStoneStorage = 20;
    public int maxFoodStorage = 20;
    int requiredWood;
    int requiredStone;
    int curWood;
    int curStone;
    public int woodStorage;
    int displayWoodStorage;
    public int stoneStorage;
    int displayStoneStorage;
    int availableWood = 0;
    int freeCharacters;
    float buildingProgress;
    public int buildingMaxProgress = 100;
    bool isBuilding;

    public Dictionary<string, Transform> buildingsDict;

    private void Awake() 
    {
        buildingsDict = buildings.ToDictionary(a => a.name, a => a.transform);
        Messenger<(string buildingId, int requiredWoodMessage, int requiredStoneMessage)>.AddListener(GameEvent.START_CRAFT, OnStartCraft);
        Messenger<(string resourceType, string taskType, int amount, GameObject character)>.AddListener(GameEvent.FINISH_TASK, OnFinishTask);
        Messenger<int>.AddListener(GameEvent.TREE_RESPAWN, OnTreeRespawn);
    }
    // Start is called before the first frame update
    void Start()
    {
        status = ManagerStatus.Collecting;
        CheckAvailableWood();
        StartCollecting();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            if (buildingProgress < buildingMaxProgress)
            {
                buildingProgress += Time.deltaTime * craftSpeed;
            }
            else
            {
                buildingProgress = buildingMaxProgress;
                isBuilding = false;
                status = ManagerStatus.Collecting;
                StartCollecting();
            }
            Messenger<float>.Broadcast(GameEvent.SET_BUILDING_PROGRESS, buildingProgress);
        }
    }

    void OnFinishTask((string resourceType, string taskType, int amount, GameObject character) message)
    {
        if (status == ManagerStatus.Crafting)
            CreateCharacterTask(message.character);
        else if (status == ManagerStatus.Collecting)
            RecieveResources(message);
    }

    void OnStartCraft((string buildingId, int requiredWood, int requiredStone) message)
    {
        buildingProgress = 0;
        status = ManagerStatus.Crafting;
        curBuilding = buildingsDict[message.buildingId];
        requiredWood = message.requiredWood;
        requiredStone = message.requiredStone;
        for (int i = 0; i < characters.Count; i++)
        {
            CreateCharacterTask(characters[i]);
        }
    }

    void CreateCharacterTask(GameObject character)
    {
        CharacterController characterCtrl = character.GetComponent<CharacterController>();
        if (characterCtrl.status == CharacterController.CharacterStatus.Free)
        {
            if (WoodRequirementCheck(characterCtrl))
                return;
            else if (StoneRequirementCheck(characterCtrl))
                return;
            else
                CheckBuildingStart();
        }
    }

    void RecieveResources((string resourceType, string taskType, int amount, GameObject character) message)
    {
        // if (message.resourceType == "Wood")
        //     woodStorage += message.amount;
        // else if (message.resourceType == "Stone")
        //     stoneStorage += message.amount;

        CollectTask(message.character);
    }

    void CheckBuildingStart()
    {
        freeCharacters++;
        if (freeCharacters == characters.Count)
            isBuilding = true;
    }

    void StartCollecting()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            CollectTask(characters[i]);
        }
    }

    void OnTreeRespawn(int volume)
    {
        availableWood += volume;
    }

    void CollectTask(GameObject character)
    {
        CharacterController characterCtrl = character.GetComponent<CharacterController>();
        if (characterCtrl.status == CharacterController.CharacterStatus.Free)
        {
            if (WoodStorageCheck(characterCtrl))
                return;
            else if (StoneStorageCheck(characterCtrl))
                return;
            else
                status = ManagerStatus.Waiting;
        }
    }

    bool WoodStorageCheck(CharacterController character)
    {
        if (woodStorage < maxWoodStorage && availableWood > 0)
        {
            (string resourceType, string taskType, int amount, Transform targetTransform) message;
            int amount = 0;
            
            int emptySpace = maxWoodStorage - woodStorage;
            if (emptySpace > characterVolume)
                amount = characterVolume;
            else
                amount = characterVolume;

            woodStorage += amount;
            availableWood -= amount;
            message = ("Wood", "StorageCollection", amount, buildingsDict["woodStorage"].transform);
            character.SendMessage("OnStartTask", message);
            return true;
        }
        return false;
    }

    bool StoneStorageCheck(CharacterController character)
    {
        if (stoneStorage < maxStoneStorage)
        {
            (string resourceType, string taskType, int amount, Transform targetTransform) message;
            int amount = 0;
            
            int emptySpace = maxStoneStorage - stoneStorage;
            if (emptySpace > characterVolume)
                amount = characterVolume;
            else
                amount = characterVolume;

            stoneStorage += amount;
            message = ("Stone", "StorageCollection", amount, buildingsDict["stoneStorage"].transform);
            character.SendMessage("OnStartTask", message);
            return true;
        }
        return false;
    }
    bool WoodRequirementCheck(CharacterController character)
    {
        if (curWood < requiredWood)
        {
            (string resourceType, string taskType, int amount, Transform targetTransform) message;
            int amount = 0;
            if (woodStorage > 0)
            {
                if (requiredWood < woodStorage && requiredWood < characterVolume)
                    amount = requiredWood;
                else if (woodStorage > characterVolume)
                    amount = characterVolume;
                else
                    amount = woodStorage;
                
                woodStorage -= amount;
                requiredWood -= amount;
                message = ("WoodStorage", "CraftCollection", amount, curBuilding.transform);
            }
            else
            {
                if (availableWood > 0)
                {
                    if (availableWood < requiredWood && requiredWood < characterVolume)
                        amount = availableWood;
                    else if (requiredWood > characterVolume)
                        amount = characterVolume;
                    else
                        amount = requiredWood;

                    requiredWood -= amount;
                    availableWood -= amount;
                    message = ("Wood", "CraftCollection", amount, curBuilding.transform);
                }
                else
                    return false;
            }
            character.SendMessage("OnStartTask", message);
            return true;
        }
        return false;
    }

    void CheckAvailableWood()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Wood");
        foreach (GameObject woodObj in resources)
        {
            int freeWood = woodObj.GetComponent<TreeCtrl>().freeVolume;
            if (freeWood > 0)
                availableWood += freeWood;
        }
    }

    bool StoneRequirementCheck(CharacterController character)
    {
        if (curStone < requiredStone)
        {
            (string resourceType, string taskType, int amount, Transform targetTransform) message;
            int amount = 0;
            if (stoneStorage > 0)
            {
                if (requiredStone < stoneStorage && requiredStone < characterVolume)
                    amount = requiredStone;
                else if (stoneStorage > characterVolume)
                    amount = characterVolume;
                else
                    amount = stoneStorage;
                
                stoneStorage -= amount;
                requiredStone -= amount;
                message = ("StoneStorage", "CraftCollection", amount, curBuilding.transform);
            }
            else
            {
                if (requiredStone > characterVolume)
                    amount = characterVolume;
                else
                    amount = requiredStone;

                requiredStone -= amount;
                message = ("Stone", "CraftCollection", amount, curBuilding.transform);
            }
            character.SendMessage("OnStartTask", message);
            return true;
        }
        return false;
    }
}
