using UnityEngine;

public class UIFallIngSprite : MonoBehaviour
{
    public float Speed;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _rect.anchoredPosition += Vector2.down * Speed * Time.deltaTime;

        if (_rect.anchoredPosition.y < - Screen.height)
        {
            Destroy(gameObject);
        }
    }
}
