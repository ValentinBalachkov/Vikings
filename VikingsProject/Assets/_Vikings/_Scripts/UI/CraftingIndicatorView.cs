using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIndicatorView : MonoBehaviour
{
    [SerializeField] private Image _indicatorImage;
    private int _maxCount;

    public void Setup(int maxCount)
    {
        DebugLogger.SendMessage("ASDASDASD", Color.green);
        gameObject.SetActive(true);
        _indicatorImage.fillAmount = 0;
        _maxCount = maxCount;
        StartCoroutine(UpdateIndicatorCoroutine());
    }

    private IEnumerator UpdateIndicatorCoroutine()
    {
        int currentTime = 0;
        DebugLogger.SendMessage("2222222", Color.green);
        while (currentTime < _maxCount)
        {
            yield return new WaitForSeconds(1f);
            _indicatorImage.fillAmount = (float)currentTime / (float)_maxCount;
            currentTime++;
        }
        gameObject.SetActive(false);
    }
    
}
