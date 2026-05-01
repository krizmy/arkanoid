using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour, IPointerDownHandler
{
    public static event Action OnBallFalledDown;
    public float JumpForce;
    public Platform Platform;

    private Rigidbody2D _rigidbody;
    private Vector3 _reflectedDirection;
    private Vector3 _startBallPosition;
    private Vector3 _startPlatformPosition;
    private bool _ballOnPlatform = true;
    private bool _accelerate = true;
    private bool _pausedDirection;
    private Coroutine _launchDelay;
    private Vector3 _savedDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _startBallPosition = transform.position;
        _startPlatformPosition = Platform.transform.position;

        GameManager.OnPlayerAttemptsEnded += StopBall;
        GameManager.OnBlocksCountEnded += StopBall;
        PauseManager.OnGamePaused += PauseBall;
        PauseManager.OnGameResumed += ResumeBall;
        PanelsManager.OnPanelActivated += StopBall;
    }

    private void StopBall()
    {
        JumpForce = 0;

        GameManager.OnPlayerAttemptsEnded -= StopBall;
        GameManager.OnBlocksCountEnded -= StopBall;
    }
    private void Update()
    {
        ConnectPositions();
    }

    private void PauseBall()
    {
        _savedDirection = _rigidbody.linearVelocity;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0;
        _pausedDirection = true;
    }

    private void ResumeBall()
    {
        _rigidbody.linearVelocity = _savedDirection;
    }

    private void OnDestroy()
    {
        PauseManager.OnGamePaused -= PauseBall;
        PauseManager.OnGameResumed -= ResumeBall;
        PanelsManager.OnPanelActivated -= StopBall;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 collisionPoint = collision.contacts[0].normal;
        _rigidbody.linearVelocity = Vector3.Reflect(_reflectedDirection, collisionPoint).normalized * JumpForce;

        if (collision.gameObject.CompareTag("VerticalBorders"))
        {
            ChangeVerticalBordersColor();
        }

        if (collision.gameObject.CompareTag("HorizontalBorders"))
        {
            ChangeHorizontalBordersColor();
        }

        FixBallDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            ReturnObjToStartPos();

            OnBallFalledDown?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_rigidbody.linearVelocity.magnitude) < JumpForce - 0.5f)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * JumpForce; 
        }
        
        _reflectedDirection = _rigidbody.linearVelocity.normalized * JumpForce;
    }
    
    private void ReturnObjToStartPos()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        transform.position = _startBallPosition;
        Platform.transform.position = _startPlatformPosition;
        _ballOnPlatform = true;

    }

    private void ConnectPositions()
    {
        if (_ballOnPlatform)
        {
            transform.position = new Vector2(Platform.transform.position.x, _startBallPosition.y);
        }
    }

    private void FixBallDirection()
    {
        float minYvelocity = 1f;
        Vector2 YballVelocity = _rigidbody.linearVelocity;
        if (Mathf.Abs(YballVelocity.y) < minYvelocity)
        {
            YballVelocity.y = YballVelocity.y > 0 ? minYvelocity : -minYvelocity;
            _rigidbody.linearVelocity = YballVelocity.normalized * JumpForce;
        }

        float minXvelocity = 1f;
        Vector2 XballVelocity = _rigidbody.linearVelocity;

        if (Mathf.Abs(XballVelocity.x) < minXvelocity)
        {
            XballVelocity.x = XballVelocity.x > 0 ? minXvelocity : -minXvelocity;

            _rigidbody.linearVelocity = XballVelocity.normalized * JumpForce;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_ballOnPlatform && !PauseManager.PausedState)
        {
            _rigidbody.linearVelocity = (Vector2.up + new Vector2(Random.Range(-1, 2), 0)) * JumpForce;
            _ballOnPlatform = false;
        }
    }
    
    private void ChangeHorizontalBordersColor()
    {
        GameObject[] horizontalBorders = GameObject.FindGameObjectsWithTag("HorizontalBorders");
        Color newColor = new Color(Random.value, Random.value, Random.value);
        foreach (var border in horizontalBorders)
        {
            Image borderRender = border.GetComponent<Image>();
            borderRender.color = newColor;
        }
    }

    private void ChangeVerticalBordersColor()
    {
        GameObject[] verticalBorders = GameObject.FindGameObjectsWithTag("VerticalBorders");
        Color newColor = new Color(Random.value, Random.value, Random.value);
        foreach (var border in verticalBorders)
        {
            Image borderRender = border.GetComponent<Image>();
            borderRender.color = newColor;
        }
    }
}
