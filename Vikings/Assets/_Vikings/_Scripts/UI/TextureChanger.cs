using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    [SerializeField] private MaskedImage _mask;

    private Texture2D _dynamicTexture;

    public void ChangeTexture(RectTransform buttonTransform)
    {
        Texture2D texture = _mask.sprite.texture;
        _dynamicTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
      

        for (int x = 0; x < _dynamicTexture.width; x++)
        {
            for (int y = 0; y < _dynamicTexture.height; y++)
            {
                if (x <= buttonTransform.position.x - (buttonTransform.sizeDelta.x/2) || x >= buttonTransform.position.x + (buttonTransform.sizeDelta.x/2) 
                    || y <= buttonTransform.position.y - (buttonTransform.sizeDelta.y/2)
                    || y >= buttonTransform.position.y + (buttonTransform.sizeDelta.y)/2)
                {
                    continue;
                }
                _dynamicTexture.SetPixel(x, y, Color.clear);
            }
        }

        _dynamicTexture.Apply();

        Sprite newSprite = Sprite.Create(_dynamicTexture, new Rect(0, 0, _dynamicTexture.width, _dynamicTexture.height), new Vector2(0.5f, 0.5f));
        _mask.sprite = newSprite;
    }

    public void DisableBackground()
    {
        _mask.gameObject.SetActive(false);
    }
}
