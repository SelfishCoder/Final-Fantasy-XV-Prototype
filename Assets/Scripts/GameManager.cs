using System;
using UnityEngine;
using SelfishCoder.Core;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int maxGamePlayTime = 45; 
    public event Action OnGameStart;
    public event Action OnGameEnd;
    public GameState gameState;
    
    private void Start()
    {
        OnGameStart?.Invoke();
        Instance.OnGameEnd += OnGameEnded;
        gameState = GameState.GamePlay;
    }

    private void Update()
    {
        if (gameState == GameState.Ended) return;
        
        if (EnemyManager.Instance.aliveEnemyCount <= 0 && EnemyManager.Instance.waveIndex >= 1)
        {
            EnemyManager.Instance.StartEnemyWave();
        }

        if (!(Time.time >= maxGamePlayTime)) return;
        gameState = GameState.Ended;
        Instance.OnGameEnd?.Invoke();
    }
    
    private void OnDestroy()
    {
        Instance.OnGameEnd -= OnGameEnded;
    }

    private void OnGameEnded()
    {
        StopAllCoroutines();
        gameState = GameState.Ended;
    }
}