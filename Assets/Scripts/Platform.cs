using UnityEngine;

public class Platform : MonoBehaviour
{
    [Range(10, 50)]public float Speed;

    private Rigidbody2D _rigidbody;
    private bool _control = true;
    private FixedJoystick _joystick;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _joystick = FindFirstObjectByType<FixedJoystick>();
        GameManager.OnPlayerAttemptsEnded += StopPlatform;
        GameManager.OnBlocksCountEnded += StopPlatform;
        PanelsManager.OnPanelActivated += StopPlatform;

        PauseManager.OnGamePaused += PausePlatform;
        PauseManager.OnGameResumed += ResumePlatform;
    }

    private void StopPlatform()
    {
        Speed = 0f;
        GameManager.OnPlayerAttemptsEnded -= StopPlatform;
        GameManager.OnBlocksCountEnded -= StopPlatform;
    }

    private void FixedUpdate()
    {
        if (_control)
        {
            float horizontal = _joystick.Horizontal;
            Vector2 newPosition = _rigidbody.position + (Vector2.right * horizontal * Speed * Time.fixedDeltaTime);
            newPosition.x = Mathf.Clamp(newPosition.x, -11.39f, 29.72f);
            _rigidbody.MovePosition(newPosition);
        }
    }
     
    private void PausePlatform()
    {
        _control = false;
    }

    private void ResumePlatform()
    {
        _control = true;
    }

    private void OnDestroy()
    {
        PauseManager.OnGamePaused -= PausePlatform;
        PauseManager.OnGameResumed -= ResumePlatform;
        PanelsManager.OnPanelActivated -= StopPlatform;
    }
}
