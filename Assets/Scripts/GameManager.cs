using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnBlocksCountUpdated;
    public static event Action OnBlocksCountEnded;
    public static event Action<int> OnPlayerAttemptsUpdated;
    public static event Action OnPlayerAttemptsEnded;

    private List<GameObject> _blocks = new List<GameObject>();
    private int _playerAttempts = 10;
    private PanelsManager _panelsManager;

    private void Awake()
    {
        _panelsManager = FindFirstObjectByType<PanelsManager>();
        StartCoroutine(Delay());
        Ball.OnBallFalledDown += HandlleAttempts;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(CheckBlocksCount());
        OnPlayerAttemptsUpdated?.Invoke(_playerAttempts);
    }

    private void HandlleAttempts()
    {
        _playerAttempts--;
        OnPlayerAttemptsUpdated?.Invoke(_playerAttempts);
        if (_playerAttempts < 1)
        {
            _panelsManager.ActivateLoseLevelPanel();
            OnPlayerAttemptsEnded?.Invoke();
            return;
        }
    }

    private void OnDestroy()
    {
        Ball.OnBallFalledDown -= HandlleAttempts;
    }

    public void SetBlocks(List<GameObject> blocks)
    {
        _blocks = blocks;
    }

    private IEnumerator CheckBlocksCount()
    {
        OnBlocksCountUpdated?.Invoke(_blocks.Count);
        while (_blocks.Count > 0)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i] == null)
                {
                    _blocks.RemoveAt(i);
                    OnBlocksCountUpdated?.Invoke(_blocks.Count);
                }
            }
            yield return null;
        }

        if (_blocks.Count == 0)
        {
            _panelsManager.ActivateWinLevelPanel();
            OnBlocksCountEnded?.Invoke();
        }
    }
}
