using TMPro;
using UnityEngine;

public class FPSCheck : MonoBehaviour
{
    private TMP_Text _text;
    private int _fps =0;
    private float _timer = 1;
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        Application.targetFrameRate = 140;
    }

    // Update is called once per frame
    void Update()
    {
        _fps++;
        _timer -= Time.deltaTime;
        
        if (_timer <= 0)
        {
            _text.text = _fps.ToString();
            _fps = 0;
            _timer = 1;
        }
    }
}
