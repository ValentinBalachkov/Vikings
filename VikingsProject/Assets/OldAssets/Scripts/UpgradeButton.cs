using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] TMP_Text description;
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text resourceText;
    [SerializeField] Image icon;
    [SerializeField] Button upgradeButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init(string[][] buttonData)
    {
        string[] cuted_rowLocalization = buttonData[0];
        string[] cuted_rowData = buttonData[1];

        label.text = cuted_rowLocalization[1];
        description.text = cuted_rowLocalization[2];
        resourceText.text = cuted_rowData[1];
    }
}
