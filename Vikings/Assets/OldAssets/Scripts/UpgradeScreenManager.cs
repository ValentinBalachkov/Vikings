using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UpgradeScreenManager : MonoBehaviour
{
    [SerializeField] GameObject upgradeButtonPrefab;
    [SerializeField] GameObject viewportContent;
    [SerializeField] Button closeButton;
    private RectTransform rt;
    private string[] rowsLocalization, rowsData;

    void Awake()
    {
        TextAsset upgradeLocalizationData=(TextAsset)Resources.Load("UpgradesLocalization");
        string localization_txt = upgradeLocalizationData.text;
        rowsLocalization = localization_txt.Split('\n');

        TextAsset upgradeData=(TextAsset)Resources.Load("Upgrades");
        string data_txt = upgradeData.text;
        rowsData = data_txt.Split('\n');

        closeButton.onClick.AddListener(closeUpgradeScreen);
    }

    // Start is called before the first frame update
    void Start()
    {
        rt = viewportContent.GetComponent<RectTransform>();
        float butHeight = upgradeButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x,  butHeight * (rowsLocalization.Length - 1));

        for (int i = 1; i < rowsLocalization.Length - 1; i++)
        {
            GameObject craftBut = Instantiate(upgradeButtonPrefab, viewportContent.transform);
            string[] cuted_rowLocalization = Regex.Split(rowsLocalization[i], ";");
            string[] cuted_rowData = Regex.Split(rowsData[i], ";");
            string[][] buttonData = new string[][] {cuted_rowLocalization, cuted_rowData};
            craftBut.SendMessage("Init", buttonData);
        }
    }

    void closeUpgradeScreen()
    {
        Messenger.Broadcast(GameEvent.ENABLE_MAINSCREEN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
