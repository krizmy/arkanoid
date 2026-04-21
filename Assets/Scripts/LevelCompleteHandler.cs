using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelCompleteHandler
{
    public static void CompleteLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel >= ProgressService.UnlockedLevel)
        {
            ProgressService.UnlockedLevel = currentLevel + 1;
        }
    }
}
