using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("GridSpawner")]
    public GameObject BlockPrefab;
    public int Rows;
    public int Columns;
    public Vector3 StartGridPosition;
    public float OffsetX;
    public float OffsetY;

    [Header("CircleSpawner")]
    public int BlocksCount;
    public float CircleRadius;
    public Vector2 CircleStartSpawnPosition;

    [Header("ZSpawner")]
    public int BlocksPerLine;
    public float BlockSpacing;
    public Vector2 ZStartSpawnPosition;

    private GameManager _gameManager;
    private List<GameObject> _spawnedBlocks = new List<GameObject>();

  

    public void SpawnGridBlocks()
    {
        _gameManager = FindFirstObjectByType<GameManager>();

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Vector3 spawnPosition = StartGridPosition + new Vector3(j * OffsetX, i * OffsetY);
                GameObject block = Instantiate(BlockPrefab, spawnPosition, Quaternion.identity);
                _spawnedBlocks.Add(block);
                _gameManager.SetBlocks(_spawnedBlocks);
            }
        }
    }

    public void SpawnCircleBlocks()
    {
        _gameManager = FindFirstObjectByType<GameManager>();

        float angle = 360 / BlocksCount;
        for (int i = 0; i < BlocksCount; i++)
        {
            float angleRad = i * angle * Mathf.Deg2Rad;
            float x = CircleRadius * Mathf.Cos(angleRad);
            float y = CircleRadius * Mathf.Sin(angleRad);
            Vector2 spawnPosition = CircleStartSpawnPosition + new Vector2(x, y);
            GameObject block = Instantiate(BlockPrefab, spawnPosition, Quaternion.identity);
            _spawnedBlocks.Add(block);
            _gameManager.SetBlocks(_spawnedBlocks);
        }
    }

    public void SpawnZBlocks()
    {
        _gameManager = FindFirstObjectByType<GameManager>();

        for (int i = 0; i < BlocksPerLine; i++)
        {
            Vector2 spawnPosition = ZStartSpawnPosition + new Vector2(i * BlockSpacing, 0);
            GameObject block = Instantiate(BlockPrefab, spawnPosition, Quaternion.identity);
            _spawnedBlocks.Add(block);
            _gameManager.SetBlocks(_spawnedBlocks);
        }

        for (int i = 1; i < BlocksPerLine; i++)
        {
            float x = (BlocksPerLine - 0.5f) * BlockSpacing - BlockSpacing * i;
            float y = -BlockSpacing * i;
            Vector2 spawnPosition = ZStartSpawnPosition + new Vector2(x, y);
            GameObject block = Instantiate(BlockPrefab, spawnPosition, Quaternion.identity);
            _spawnedBlocks.Add(block);
            _gameManager.SetBlocks(_spawnedBlocks);
        }

        Vector2 bottomPosition = ZStartSpawnPosition + new Vector2(ZStartSpawnPosition.x, -BlocksPerLine * BlockSpacing);
        for (int i = 0; i < BlocksPerLine; i++)
        {
            Vector2 spawnPosition = bottomPosition + new Vector2(i * BlockSpacing, 0);
            GameObject block = Instantiate(BlockPrefab, spawnPosition, Quaternion.identity);
            _spawnedBlocks.Add(block);
            _gameManager.SetBlocks(_spawnedBlocks);
        }
    }
}
