using System;
using UnityEngine;
using SelfishCoder.Core;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyPoolSize = 50;
    [SerializeField] private List<GameObject> enemyPool = new List<GameObject>();
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> enemyWayPoints = new List<Transform>();
    [SerializeField] private List<Wave> waves = new List<Wave>();
    private readonly Dictionary<Transform,Transform> wayPoints = new Dictionary<Transform, Transform>();
    
    public int spawnedEnemyCount = 0;
    public int aliveEnemyCount = 0;
    public int waveIndex = 0;
    
    public event Action<Enemy> OnEnemyDestroyed;

    private void InitWayPoints()
    {
        for (int i = 0; i < 4; i++)
        {
            wayPoints.Add(enemySpawnPoints[i],enemyWayPoints[i]);
        }    
    }

    private void InitEnemyPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, EnemyManager.Instance.gameObject.transform, true);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }
    
    private void Start()
    {
        InitWayPoints();
        InitEnemyPool(enemyPoolSize);
    }

    private void SpawnEnemyAt(Transform location)
    {
        foreach (GameObject enemyInstance in Instance.enemyPool)
        {
            if (enemyInstance.activeInHierarchy) continue;
            GameObject enemyGameObject = enemyInstance;
            enemyGameObject.transform.SetPositionAndRotation(location.position, Quaternion.Euler(0,-30,0));
            enemyGameObject.SetActive(true);
                
            Enemy enemy = enemyGameObject.GetComponent<Enemy>(); 
            enemy.SetTarget(wayPoints[location]);
            enemy.Health = waves[waveIndex].enemyHealth;
            enemy.Speed = waves[waveIndex].enemySpeed;
                
            aliveEnemyCount++;
            spawnedEnemyCount++;
            return;
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        OnEnemyDestroyed?.Invoke(enemy);
        enemy.gameObject.SetActive(false);
        aliveEnemyCount--;
    }

    public void StartEnemyWave()
    {
        spawnedEnemyCount = 0;
        StartCoroutine(SpawnEnemy(waveIndex));
    }
    
    private IEnumerator SpawnEnemy(int index)
    {
        while (spawnedEnemyCount < waves[index].EnemyCount)
        {
            foreach (Transform spawnPoint in enemySpawnPoints)
            {
                SpawnEnemyAt(spawnPoint);
            }
            yield return new WaitForSeconds(Random.Range(1.5f,2.5f));
        }
        waveIndex++;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += StartEnemyWave;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= StartEnemyWave;
    }

}