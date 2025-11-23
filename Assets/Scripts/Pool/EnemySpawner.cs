using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private LineRenderer patrolPath;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private Transform[] spawnPoints;

    private float lastSpawnTime;
    private int activeEnemies = 0;

    public event Action<int> OnScoreEnemy;

    private void Start()
    {
        UpdateManager.Instance.RegisterUpdate(OnUpdate);
    }

    private void OnUpdate()
    {
        if (Time.time - lastSpawnTime >= spawnInterval && activeEnemies < maxEnemies)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnEnemy()
    {
        var enemyObj = enemyPool.GetObject();

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            spawnPosition = randomSpawnPoint.position;
            spawnRotation = randomSpawnPoint.rotation;
        }

        enemyObj.transform.position = spawnPosition;
        enemyObj.transform.rotation = spawnRotation;

        var enemy = enemyObj.GetComponent<Enemy>();
        var health = enemyObj.GetComponent<HealthAbility>();
        var shootAbility = enemyObj.GetComponent<ShootAbility>();

        if (shootAbility != null)
        {
            shootAbility.SetBulletPool(bulletPool);
        }

        enemy.SetPatrolPath(patrolPath);

        health.OnDeath -= () => OnEnemyDeath(enemyObj);
        health.OnDeath += () => OnEnemyDeath(enemyObj);

        activeEnemies++;
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        activeEnemies--;

        var enemyScore = enemy.GetComponent<Enemy>().ScoreCost;
        OnScoreEnemy?.Invoke(enemyScore);

        var poolable = enemy.GetComponent<IPoolable>();
        poolable?.ReturnToPool();
    }

    private void OnDestroy()
    {
        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
    }
}