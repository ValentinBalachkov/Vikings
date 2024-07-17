using UnityEngine;
using UnityEngine.UI;

public class MaskedImage : Image
{
    public override bool Raycast(Vector2 sp, Camera eventCamera)
    {
        Texture2D tex = sprite.texture;
        return tex.GetPixelBilinear(sp.x / rectTransform.rect.width, sp.y / rectTransform.rect.height).a > 0;
    }
}
