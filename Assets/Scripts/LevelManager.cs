using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int _behaviour;
    public Block Block;
    public BlockSpawner BlockSpawner;
    public List<Color> BackgroundColors = new List<Color>();

    private delegate void _blockBehaviours();

    private delegate void _spawnBehaviours();

    private void Awake()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SetupLevel(levelIndex);
        ChangeBackgroundColor();

        Block.gameObject.SetActive(false);
    }

    private void ChangeBackgroundColor()
    {
        Color newColor = BackgroundColors[Random.Range(0, BackgroundColors.Count)];
        Camera.main.backgroundColor = newColor;
    }

    private void SetupLevel(int levelIndex)
    {
        _blockBehaviours[] blockBehaviours = new _blockBehaviours[]
        {
            Block.Behaviour1, Block.Behaviour2, Block.Behaviour3
        };

        _spawnBehaviours[] spawnBehabiours = new _spawnBehaviours[]
        {
            BlockSpawner.SpawnGridBlocks, BlockSpawner.SpawnCircleBlocks, BlockSpawner.SpawnZBlocks
        };

        LevelCombination levelCombination;

        if (LevelCombinationsStorage.Combinations.ContainsKey(levelIndex))
        {
            levelCombination = LevelCombinationsStorage.Combinations[levelIndex];
        }
        else
        {
            levelCombination = GenerateUniqueCombination();
            LevelCombinationsStorage.Combinations.Add(levelIndex, levelCombination);
        }

        _behaviour = levelCombination.SpawnIndex;
        spawnBehabiours[levelCombination.BehaviourIndex]();
        blockBehaviours[levelCombination.SpawnIndex]();
    }

    public static int ReturnBehaviourIndex()
    {
        return _behaviour;
    }

    private LevelCombination GenerateUniqueCombination()
    {
        int maxCombinations = 3 * 3;
        if (LevelCombinationsStorage.Combinations.Count >= maxCombinations)
        {
            return new LevelCombination(0, 0);
        }

        int spawn, behaviour;
        do
        {
            spawn = Random.Range(0, 3);
            behaviour = Random.Range(0, 3);
            _behaviour = behaviour;
        } while (IsCombinationUsed(spawn, behaviour));

        return new LevelCombination(spawn, behaviour);
    }


    private bool IsCombinationUsed(int spawn, int behaviour)
    {
        foreach (var combo in LevelCombinationsStorage.Combinations.Values)
        {
            if (combo.BehaviourIndex == behaviour && combo.SpawnIndex == spawn)
            {
                return true;
            }
        }

        return false;
    }
}