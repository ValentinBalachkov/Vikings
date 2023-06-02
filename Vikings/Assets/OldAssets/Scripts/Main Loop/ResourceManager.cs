using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] TMP_Text woodText;
    [SerializeField] TMP_Text stoneText;
    [SerializeField] TMP_Text foodText;

    CraftManager craftManager;
    Vector3 offset = new Vector3 (0, 100, 0);

    int woodStorage;
    int stoneStorage;
    // Start is called before the first frame update

    private void Awake() 
    {
        craftManager = GetComponent<CraftManager>();
        // Messenger<(string resourceType, int amount, Transform targetTransform)>.AddListener(GameEvent.FINISH_TASK, OnFinishTask);
        Messenger<(string buildingId, int requiredWoodMessage, int requiredStoneMessage)>.AddListener(GameEvent.START_CRAFT, OnStartCraft);
    }
    void Start()
    {
        
    }

    void OnStartCraft((string buildingId, int requiredWoodMessage, int requiredStoneMessage) message)
    {
        // Camera camera= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // var objective = GameObject.FindGameObjectWithTag("Finish");
        // var objectiveImage = GameObject.FindGameObjectWithTag("Respawn");
        // Vector3 objectiveScreenPos = camera.WorldToScreenPoint(objective.transform.position);
        // objectiveImage.transform.position = objectiveScreenPos + offset;
    }

    void OnFinishTask((string resourceType, int amount, Transform targetTransform) message)
    {
        if (message.resourceType == "Wood")
            woodText.text = string.Format("{0}/{1}", woodStorage - message.amount, craftManager.maxWoodStorage);
    }
    void ChangeStoneText(int newValue)
    {
        stoneText.text = string.Format("{0}/{1}", newValue, craftManager.maxStoneStorage);
    }
    void ChangeWoodText(int newValue)
    {
        woodText.text = string.Format("{0}/{1}", newValue, craftManager.maxWoodStorage);
    }
    void ChangeFoodText(int newValue)
    {
        foodText.text = string.Format("{0}/{1}", newValue, craftManager.maxFoodStorage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
