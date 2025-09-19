using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFitter : MonoBehaviour
{
    public enum Mode { Fit, Fill } // Fit=모두 보이게, Fill=화면 꽉 채움(잘릴 수 있음)
    public Mode scaleMode = Mode.Fill;
    SpriteRenderer sr;
    Vector2Int lastRes;
    float lastSize;
    float lastAspect;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        Apply();
    }

    void Update()
    {
        var cam = Camera.main;
        if (!cam || !sr || !sr.sprite) return;

        if (lastRes.x != Screen.width || lastRes.y != Screen.height ||
            lastSize != cam.orthographicSize || lastAspect != cam.aspect)
        {
            Apply();
        }
    }

    void Apply()
    {
        var cam = Camera.main;
        if (!cam || !sr || !sr.sprite) return;

        lastRes = new Vector2Int(Screen.width, Screen.height);
        lastSize = cam.orthographicSize;
        lastAspect = cam.aspect;

        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size; // 월드 단위 크기
        float scaleX = worldW / spriteSize.x;
        float scaleY = worldH / spriteSize.y;

        float scale = (scaleMode == Mode.Fill) ? Mathf.Max(scaleX, scaleY) : Mathf.Min(scaleX, scaleY);
        transform.localScale = new Vector3(scale ,scale, 1f);

        // 카메라 중심에 정렬
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
    }
}
