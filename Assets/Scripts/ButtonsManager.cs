using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class ButtonsManager : MonoBehaviour
{
    public static event Action OnButtonPressed;

    public Button NextLevelButton;
    public Button RestartLevelButton;
    public Button[] ExitMenuButtons;

    private float _animationTime = 0.3f;
    private float _targetSize = 0.9f;

    private void Awake()
    {
        NextLevelButton.onClick.AddListener(LoadNextLevel);
        RestartLevelButton.onClick.AddListener(RestartLevel);
        LoadMenu(ExitMenuButtons);
    }

    private void LoadNextLevel()
    {        
        OnButtonPressed?.Invoke();
        NextLevelButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
            OnComplete(() =>
            {
                NextLevelButton.transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.InOutQuad).
                OnComplete(() =>
                {
                    StartCoroutine(Delay());
                });
            });

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(LoadingPanelsManager.AnimationTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            NextLevelButton.transform.DOKill();
        }
    }

    private void RestartLevel()
    {
        OnButtonPressed?.Invoke();
        RestartLevelButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
           OnComplete(() =>
           {
               RestartLevelButton.gameObject.transform.DOScale(Vector3.one, _animationTime).
               OnComplete(() =>
               {
                   StartCoroutine(Delay());
               });
           });

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(LoadingPanelsManager.AnimationTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            RestartLevelButton.transform.DOKill();
        }
    }

    private void LoadMenu(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            buttons[i].onClick.AddListener(() =>
            {
                OnButtonPressed?.Invoke();
                buttons[index].transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
               OnComplete(() =>
               {
                   buttons[index].gameObject.transform.DOScale(Vector3.one, _animationTime).
                   OnComplete(() =>
                   {
                       StartCoroutine(Delay(index));
                   });
               });
            });
        }

        IEnumerator Delay(int index)
        {
            yield return new WaitForSeconds(LoadingPanelsManager.AnimationTime);
            SceneManager.LoadScene("Menu");
            buttons[index].transform.DOKill();
        }
    }
}
