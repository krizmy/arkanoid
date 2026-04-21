using UnityEngine;

public class UIRainSpawner : MonoBehaviour
{
    public GameObject[] Prefabs;
    public RectTransform Parent;
    public float SpawnDelay = 0.2f;
    public float MinSpeed = 50;
    public float MaxSpeed = 200;

    private float _canvasWidth;
    private float _canvasHeight;

    private void Start()
    {
        _canvasWidth = Parent.rect.width;
        _canvasHeight = Parent.rect.height;

        InvokeRepeating(nameof(Spawn), 0f, SpawnDelay);
    }

    private void Spawn()
    {
        GameObject obj = Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], Parent);
        RectTransform rect = obj.GetComponent<RectTransform>();
        float randomX = Random.Range(-_canvasWidth / 2, _canvasWidth / 2);
        float spawnY = _canvasHeight / 2 + 300f;
        rect.anchoredPosition = new Vector2(randomX, spawnY);
        rect.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        float speed = Random.Range(MinSpeed, MaxSpeed);
        obj.GetComponent<UIFallIngSprite>().Speed = speed;
    }
}
