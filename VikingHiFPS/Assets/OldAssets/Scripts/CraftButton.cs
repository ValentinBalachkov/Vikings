using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CraftButton : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] TMP_Text description;
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text woodText;
    [SerializeField] TMP_Text stoneText;
    [SerializeField] Image icon;
    [SerializeField] Button craftButton;
    string buildingId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init(string[] cuted_row)
    {
        buildingId = cuted_row[0];
        label.text = cuted_row[1];
        description.text = cuted_row[2];
        woodText.text = cuted_row[3];
        stoneText.text = cuted_row[4];
        craftButton.onClick.AddListener(StartCraft);
    }

    void StartCraft()
    {
        var messenge = (buildingId, Convert.ToInt32(woodText.text), Convert.ToInt32(stoneText.text));
        Messenger<(string buildingId, int requiredWood, int requiredStone)>.Broadcast(GameEvent.START_CRAFT, messenge);
        Messenger.Broadcast(GameEvent.ENABLE_MAINSCREEN);
    }
}
