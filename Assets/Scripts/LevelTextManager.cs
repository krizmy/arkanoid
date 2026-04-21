using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTextManager : MonoBehaviour
{
    public TMP_Text LevelText;

    private int _levelIndex;

    private void Awake()
    {
        _levelIndex = SceneManager.GetActiveScene().buildIndex;
        LevelText.text = $"Level {_levelIndex}";
    }
}
