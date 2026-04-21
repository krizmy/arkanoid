using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private Sprite _unlockedSprite;

    private float _animationTime = 0.3f;
    private float _targetSize = 0.9f;
    private Button _levelButton;

    private void Awake()
    {
        _levelButton = GetComponent<Button>();

        _levelButton.interactable = _levelIndex <= ProgressService.UnlockedLevel;
        if (_levelButton.interactable)
        {
            _levelButton.GetComponent<Image>().sprite = _unlockedSprite;
        }
        _levelButton.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        _levelButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
           OnComplete(() =>
           {
               _levelButton.transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.InOutQuad).
               OnComplete(() =>
               {
                   StartCoroutine(Delay());
               });
           });

        IEnumerator Delay()
        {
            float animationTime = 3f;
            FindFirstObjectByType<MenuManager>().ActivateLoadingScreen(animationTime);
            yield return new WaitForSeconds(animationTime);
            SceneManager.LoadScene(_levelIndex);
        }
    }
}
