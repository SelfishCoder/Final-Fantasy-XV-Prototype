﻿using System;
using UnityEngine;
using SelfishCoder.Core;

[DisallowMultipleComponent]
public class GoldManager : Singleton<GoldManager>
{
    public event Action<int> OnGoldChanged;
    private int gold;

    public int Gold
    {
        get => Instance.gold;
        private set
        {
            Instance.gold = value;
            Instance.OnGoldChanged?.Invoke(Instance.gold);
        }
    }

    private void Start()
    {
        Gold = 25;
        EnemyManager.Instance.OnEnemyDestroyed += OnEnemyDestroyed;
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.OnEnemyDestroyed -= OnEnemyDestroyed;
    }

    public void SpendGold(int amount)
    {
        Instance.Gold -= amount;
    }

    public void GainGold(int amount)
    {
        Instance.Gold += amount;
    }

    public bool HasEnoughGold(int amount)
    {
        return Instance.Gold >= amount;
    }
    
    private void OnEnemyDestroyed(Enemy enemy)
    {
        Instance.GainGold(enemy.gold);
    }

}
