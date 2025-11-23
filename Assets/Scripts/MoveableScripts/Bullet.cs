using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] 
    private int bulletDamage = 1;
    [SerializeField] 
    private float moveSpeed = 1f;
    [SerializeField] 
    private float lifeTime = 5f;
    [SerializeField]
    private GameObject exploseParticle;

    private BulletBelong bulletBelong = BulletBelong.None;
    private ObjectPool pool;
    private float spawnTime;
    private Vector2 moveDirection;

    public void Init(BulletBelong belong, Vector3 position, Quaternion rotation)
    {
        bulletBelong = belong;
        transform.position = position;
        transform.rotation = rotation;
        spawnTime = Time.time;

        moveDirection = transform.up;

        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
        UpdateManager.Instance?.RegisterUpdate(OnUpdate);
    }

    private void OnUpdate()
    {
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        if (Time.time - spawnTime > lifeTime)
            ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player") && bulletBelong == BulletBelong.Player) ||
            (other.CompareTag("Enemy") && bulletBelong == BulletBelong.Enemy))
            return;

        var health = other.GetComponent<HealthAbility>();
        if (health != null)
        {
            health.DecreaseHealth(bulletDamage);
        }

        Instantiate(exploseParticle, transform.position, transform.rotation);

        ReturnToPool();
    }

    public void SetPool(ObjectPool objectPool) => pool = objectPool;

    public void ReturnToPool()
    {
        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
        pool?.ReturnObject(gameObject);
    }

    private void OnDisable()
    {
        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
    }
}

public enum BulletBelong
{
    None,
    Enemy,
    Player
}