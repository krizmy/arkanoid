using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static bool PausedState;

    public Slider SpeedSlider;
    public GameObject PausePanel;
    public Button PauseButton;
    public Button ResumeButton;

    private CanvasGroup _panelAlpha;
    private RectTransform _panelTransform;
    private Platform _platform;
    private float _animationTime = 0.3f;
    private float _targetSize = 0.9f;

    private void Awake()
    {
        PanelsManager.OnPanelActivated += DeactivatePauseButton;
        PauseButton.onClick.AddListener(PauseGame);
        ResumeButton.onClick.AddListener(ResumeGame);

        _platform = FindFirstObjectByType<Platform>();
        _panelAlpha = PausePanel.GetComponent<CanvasGroup>();
        _panelTransform = PausePanel.GetComponent<RectTransform>();
        SpeedSlider.minValue = 10;
        SpeedSlider.maxValue = 50;
        SpeedSlider.value = _platform.Speed;

        SpeedSlider.onValueChanged.AddListener(ChangeSliderSpeed);
        PausedState = false;
    }

    private void ChangeSliderSpeed(float value)
    {
        _platform.Speed = value;
    }

    private void OnDestroy()
    {
        PanelsManager.OnPanelActivated -= DeactivatePauseButton;
    }

    private void DeactivatePauseButton()
    {
        PauseButton.gameObject.SetActive(false);        
    }

    private void PauseGame()
    {
        PauseButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
            OnComplete(() =>
            {
                PauseButton.gameObject.transform.localScale = Vector3.one;
                PauseButton.transform.DOKill();
            });

        PausedState = true;
        PauseButton.interactable = false;
        OnGamePaused?.Invoke();
        PausePanel.SetActive(true);
        Vector3 startPosition = new Vector3(-766, 391);
        _panelTransform.anchoredPosition = startPosition;
        PausePanel.transform.localScale = Vector3.zero;
        _panelAlpha.alpha = 0;

        Sequence seq = DOTween.Sequence();
        seq.Append(_panelTransform.DOAnchorPos(new Vector3(300, 0), 0.5f)).
            Join(_panelAlpha.DOFade(1f, 0.5f)).
            Join(PausePanel.transform.DOScale(1f, 0.5f)).OnComplete(() =>
            {
                

            });
    }

    private void ResumeGame()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_panelTransform.DOAnchorPos(new Vector3(-766, 391), 0.5f)).
            Join(_panelAlpha.DOFade(0f, 0.5f)).
            Join(PausePanel.transform.DOScale(0f, 0.5f)).OnComplete(() =>
            {
                OnGameResumed?.Invoke();
                PausedState = false;
                PauseButton.interactable = true;
                PausePanel.SetActive(false);
            });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CheckPanelActivity())
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Return) && CheckPanelActivity())
        {
            ResumeGame();
        }
    }

    private bool CheckPanelActivity()
    {
        return PausedState;
    }
}
