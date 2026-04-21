using UnityEngine;

public static class ProgressService 
{
    private const string LEVEL_KEY = "UnlockedLevel";

    public static int UnlockedLevel
    {
        get
        {
            if (!PlayerPrefs.HasKey(LEVEL_KEY)) return 1;
            return PlayerPrefs.GetInt(LEVEL_KEY, 1);
        }
        set
        {
            PlayerPrefs.SetInt(LEVEL_KEY, value);
            PlayerPrefs.Save();
        }
    }
}
