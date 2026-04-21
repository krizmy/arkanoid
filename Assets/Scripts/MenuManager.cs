using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup LevelsPanel;
    public CanvasGroup LoadingScreen;
    public Button OpenPanelButton;
    public Button ClosePanelButton;
    public Button QuitGameButton;

    private float _animationTime = 0.3f;
    private float _targetSize = 0.9f;

    private void Awake()
    {
        OpenPanelButton.onClick.AddListener(OpenPanel);
        ClosePanelButton.onClick.AddListener(ClosePanel);
        QuitGameButton.onClick.AddListener(QuitGame);
    }

    public void ActivateLoadingScreen(float animationTime)
    {
        LoadingScreen.gameObject.SetActive(true);
        LoadingScreen.DOFade(1, animationTime);
    }

    private void OpenPanel()
    {
        OpenPanelButton.interactable = false;
        OpenPanelButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
            OnComplete(() =>
            {
                OpenPanelButton.transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.InOutQuad).
                OnComplete(() =>
                {
                    LevelsPanel.alpha = 0;
                    LevelsPanel.gameObject.SetActive(true);
                    LevelsPanel.DOFade(1, _animationTime);
                });
            });
    }

    private void ClosePanel()
    {
        ClosePanelButton.interactable = false;
        ClosePanelButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
            OnComplete(() =>
            {
                ClosePanelButton.transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.InOutQuad).
                OnComplete(() =>
                {
                    LevelsPanel.DOFade(0, _animationTime).OnComplete(() =>
                    {
                        OpenPanelButton.interactable = true;
                        ClosePanelButton.interactable = true;
                        LevelsPanel.gameObject.SetActive(false);
                    });
                });
            });
    }

    private void QuitGame()
    {
        QuitGameButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
            OnComplete(() =>
            {
                QuitGameButton.transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.InOutQuad).
                OnComplete(() =>
                {
                    StartCoroutine(Delay());
                });
            });

        IEnumerator Delay()
        {
            float animationTime = 0.5f;
            FindFirstObjectByType<MenuManager>().ActivateLoadingScreen(animationTime);
            yield return new WaitForSeconds(animationTime);
            Application.Quit();
        }
    }
}
