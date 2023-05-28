using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CraftScreenManager : MonoBehaviour
{
    [SerializeField] GameObject craftButtonPrefab;
    [SerializeField] GameObject viewportContent;
    [SerializeField] Button closeButton;
    private RectTransform rt;
    private string[] rows;

    void Awake()
    {
        TextAsset craftData = (TextAsset)Resources.Load("CraftLocalization");
        string loc_txt = craftData.text;
        rows = loc_txt.Split('\n');

        closeButton.onClick.AddListener(closeCraftScreen);
    }

    // Start is called before the first frame update
    void Start()
    {
        rt = viewportContent.GetComponent<RectTransform>();
        float butHeight = craftButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x,  butHeight * (rows.Length - 1));

        for (int i = 1; i < rows.Length - 1; i++)
        {
            GameObject craftBut = Instantiate(craftButtonPrefab, viewportContent.transform);
            string[] cuted_row = Regex.Split(rows[i], ";");
            craftBut.SendMessage("Init", cuted_row);
        }
    }

    void closeCraftScreen()
    {
        Messenger.Broadcast(GameEvent.ENABLE_MAINSCREEN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
