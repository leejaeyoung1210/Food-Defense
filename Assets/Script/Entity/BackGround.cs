using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitBackgroundToScreen : MonoBehaviour
{
    void Start()
    {
        FitToScreen();
    }

    void FitToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

        Vector3 spriteSize = sr.sprite.bounds.size;

        Vector3 scale = transform.localScale;
        scale.x = worldScreenWidth / spriteSize.x;
        scale.y = worldScreenHeight / spriteSize.y;
        transform.localScale = scale;
    }
}
