using UnityEngine;

public class ShootAbility : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpread = 0.1f;

    private float lastFireTime;
    private BulletBelong bulletBelong;
    private bool canShoot = true;

    private void Start()
    {
        bulletBelong = CompareTag("Player") ? BulletBelong.Player : BulletBelong.Enemy;
        UpdateManager.Instance.RegisterUpdate(OnUpdate);
    }

    public void SetBulletPool(ObjectPool pool)
    {
        bulletPool = pool;
    }

    public void OnUpdate()
    {
        if (bulletPool == null || !canShoot) return;

        if (Time.time - lastFireTime >= fireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    private void Fire()
    {
        if (bulletPool == null)
        {
            Debug.LogError("BulletPool is not set in ShootAbility!");
            return;
        }

        var bulletObj = bulletPool.GetObject();
        var bullet = bulletObj.GetComponent<Bullet>();

        Quaternion spreadRotation = ApplySpread(spawnPoint.rotation);

        bullet.Init(bulletBelong, spawnPoint.position, spreadRotation);
    }

    private Quaternion ApplySpread(Quaternion originalRotation)
    {
        if (bulletSpread <= 0) return originalRotation;

        Vector3 spread = new Vector3(
            Random.Range(-bulletSpread, bulletSpread),
            Random.Range(-bulletSpread, bulletSpread),
            0
        );

        return originalRotation * Quaternion.Euler(spread);
    }

    public void StopShooting()
    {
        canShoot = false;
    }

    public void StartShooting()
    {
        canShoot = true;
    }

    private void OnDisable()
    {
        StopShooting();
    }

    private void OnEnable()
    {
        StartShooting();
    }

    private void OnDestroy()
    {
        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
    }
}