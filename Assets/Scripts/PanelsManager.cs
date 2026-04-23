using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour
{
    public static event Action OnPanelActivated;
    public CanvasGroup WinLevelPanel;
    public CanvasGroup LoseLevelPanel;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 9)
        {
            var text = WinLevelPanel.GetComponentInChildren<TMP_Text>();
            var button = WinLevelPanel.GetComponentInChildren<Button>();
            
            text.fontSize = 40;
            text.text = $"CONGERATS, YOU PASS THE GAME <3";
            button.gameObject.SetActive(false);
        }
    }

    public void ActivateWinLevelPanel()
    {
        FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().enabled = false;
        WinLevelPanel.alpha = 0;
        WinLevelPanel.gameObject.SetActive(true);
        WinLevelPanel.DOFade(1f, 1f);
        OnPanelActivated?.Invoke();
        LevelCompleteHandler.CompleteLevel();
    }

    public void ActivateLoseLevelPanel()
    {
        FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().enabled = false;
        LoseLevelPanel.alpha = 0;
        LoseLevelPanel.gameObject.SetActive(true);
        LoseLevelPanel.DOFade(1f, 1f);
        OnPanelActivated?.Invoke();//������� ����� � ������� ���� ��������
    }
}
