using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;
    private bool _active;
    private bool _fallingDown;
    private Vector3 _savedVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        PauseManager.OnGamePaused += DeactivateBlock;
        PanelsManager.OnPanelActivated += DeactivateBlock;
        PauseManager.OnGameResumed += ActivateBlock;
    }

    private void Start()
    {
        SetRandomColor();
    }

    private void ActivateBlock()
    {
        _active = false;

        if (!_fallingDown) return;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.linearVelocity = _savedVelocity;
    }

    private void DeactivateBlock()
    {
        _active = true;

        if (!_fallingDown) return;
        _savedVelocity = _rigidbody.linearVelocity;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            switch (LevelManager.ReturnBehaviourIndex())
            {
                case 0: Behaviour1(); break;
                case 1: Behaviour2(); break;
                case 2: Behaviour3(); break;
            }
        }
    }

    private IEnumerator DestroyBlock()
    {
        yield return new WaitWhile(() => _active);

        float destroyPositionY = -20f;
        while (transform.position.y > destroyPositionY)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Behaviour1()
    {
        _fallingDown = true;
        //меняет цвет и падает вниз
        if (_spriteRenderer == null) return;

        _spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        _boxCollider.isTrigger = true;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;

        StartCoroutine(DestroyBlock());
    }

    public void Behaviour2()
    {

        //уменьшаем размер в 2 раза и падает вниз
        Vector3 targetScale = Vector3.one / 2;
        float timer = 3;
        float countdown = 0;
        if (_boxCollider == null) return;
        _boxCollider.isTrigger = true;
        StartCoroutine(ReduceBlock());

        IEnumerator ReduceBlock()
        {
            while (timer > countdown)
            {
                yield return new WaitWhile(() => _active);
                countdown += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, (countdown / timer) * Time.deltaTime);
                yield return null;
            }
            _fallingDown = true;
            //_boxCollider.isTrigger = true;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            StartCoroutine(DestroyBlock());
        }
    }

    public void Behaviour3()
    {
        Vector3 startPosition = transform.position;
        Vector3 leftPosition = startPosition + Vector3.left * 1f;
        Vector3 rightPosition = startPosition + Vector3.right * 1f;

        StartCoroutine(MoveBlock());

        IEnumerator MoveBlock()
        {
            if (_boxCollider == null) yield break;
            _boxCollider.isTrigger = true;

            while (transform.position != leftPosition)
            {
                yield return new WaitWhile(() => _active);
                transform.position = Vector3.Lerp(transform.position, leftPosition, 25f * Time.deltaTime);

                yield return null;
            }
            transform.position = leftPosition;

            while (transform.position != rightPosition)
            {
                yield return new WaitWhile(() => _active);
                transform.position = Vector3.Lerp(transform.position, rightPosition, 25f * Time.deltaTime);
                yield return null;
            }
            transform.position = rightPosition;
            Destroy(gameObject);
        }
    }

    private void SetRandomColor()
    {
        _spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
    }

    private void OnDestroy()
    {
        PauseManager.OnGamePaused -= DeactivateBlock;
        PanelsManager.OnPanelActivated -= DeactivateBlock;
        PauseManager.OnGameResumed -= ActivateBlock;
    }
}
