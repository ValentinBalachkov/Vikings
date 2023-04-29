using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vikings.Building;

public class CollectingResourceView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _woodBar;
    [SerializeField] private Image _rockBar;

    [SerializeField] private TMP_Text _woodCount;
    [SerializeField] private TMP_Text _rockCount;

    public void Setup(string nameBuilding)
    {
        _woodCount.text = $"";
        _rockCount.text = $"";
        _woodBar.fillAmount = 0;
        _rockBar.fillAmount = 0;
        _name.text = nameBuilding;
        gameObject.SetActive(true);
    }

    public void UpdateView(PriceToUpgrade[] current, PriceToUpgrade[] all)
    {
        _woodCount.text = $"{current[0].count}/{all[0].count}";
        _rockCount.text = $"{current[1].count}/{all[1].count}";
        _woodBar.fillAmount = (float)current[0].count / (float)all[0].count;
        _rockBar.fillAmount = (float)current[1].count / (float)all[1].count;
    }
}
