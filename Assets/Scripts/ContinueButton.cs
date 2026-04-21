using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    private Button _continueButton;
    private float _animationTime = 0.3f;
    private float _targetSize = 0.9f;

    private void Awake()
    {
        _continueButton = GetComponent<Button>();

        _continueButton.onClick.AddListener(ContinueGame);
    }

    private void ContinueGame()
    {
        _continueButton.transform.DOScale(_targetSize, _animationTime).SetEase(Ease.InOutQuad).
           OnComplete(() =>
           {
               _continueButton.transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.InOutQuad).
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
            SceneManager.LoadScene(ProgressService.UnlockedLevel);
        }
    }
}
