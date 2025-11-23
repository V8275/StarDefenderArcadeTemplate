using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private ObjectPool[] enemyPool;

    private void Start()
    {
        bulletPool.Initialize();
        foreach (var pool in enemyPool)
            pool.Initialize();
    }
}