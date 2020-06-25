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
    [SerializeField] private List<GameObject> enemyPool = new List<GameObject>();
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> enemyWaypoints = new List<Transform>();
    [SerializeField] private List<Wave> waves = new List<Wave>();
    private Dictionary<Transform,Transform> waypoints = new Dictionary<Transform, Transform>();
    
    public int aliveEnemyCount = 0;
    public int spawnedEnemyCount;
    public int waveIndex;
    
    public event Action<Enemy> OnEnemyDestroyed;

    private void InitWaypoints()
    {
        for (int i = 0; i < 4; i++)
        {
            waypoints.Add(enemySpawnPoints[i],enemyWaypoints[i]);
        }    
    }
    
    private void Start()
    {
        InitWaypoints();
        for (int i = 0; i < 50; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.SetParent(EnemyManager.Instance.gameObject.transform);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    public void SpawnEnemyAt(Transform location)
    {
        for (int i = 0; i < Instance.enemyPool.Count; i++)
        {
            if (!Instance.enemyPool[i].activeInHierarchy)
            {
                GameObject enemyGameObject = Instance.enemyPool[i];
                enemyGameObject.transform.SetPositionAndRotation(location.position, Quaternion.Euler(0,-30,0));
                enemyGameObject.SetActive(true);
                
                Enemy enemy = enemyGameObject.GetComponent<Enemy>(); 
                enemy.SetTarget(waypoints[location]);
                enemy.Health = waves[waveIndex].enemyHealth;
                enemy.Speed = waves[waveIndex].enemySpeed;
                
                aliveEnemyCount++;
                spawnedEnemyCount++;
                return;
            }
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        OnEnemyDestroyed?.Invoke(enemy.GetComponent<Enemy>());
        enemy.SetActive(false);
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