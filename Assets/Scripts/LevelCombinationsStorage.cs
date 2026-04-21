using System.Collections.Generic;
using UnityEngine;

public static class LevelCombinationsStorage 
{
    public static Dictionary<int, LevelCombination> Combinations = new Dictionary<int, LevelCombination>();
}

public struct LevelCombination
{
    public int SpawnIndex;
    public int BehaviourIndex;

    public LevelCombination(int spawn, int behaviour)
    {
        SpawnIndex = spawn;
        BehaviourIndex = behaviour;
    }
}
