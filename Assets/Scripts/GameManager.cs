using System;
using UnityEngine;
using SelfishCoder.Core;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float gameTime = 0;
    public event Action OnGameStart;
    public event Action OnGameEnd;
    public GameState gameState;
    
    private void Start()
    {
        OnGameStart?.Invoke();
        Instance.OnGameEnd += OnGameEnded;
        gameState = GameState.Gameplay;
    }

    private void Update()
    {
        if (gameState == GameState.Ended) return;
        if (EnemyManager.Instance.aliveEnemyCount <= 0 && EnemyManager.Instance.waveIndex >= 1)
        {
            EnemyManager.Instance.StartEnemyWave();
        }
        gameTime = Time.time;
        
        if (gameTime>=45)
        {
            gameState = GameState.Ended;
            Instance.OnGameEnd?.Invoke();
        }
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

public enum GameState
{
    Gameplay, 
    
    Ended
}