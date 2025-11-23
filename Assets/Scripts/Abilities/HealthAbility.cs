using System;
using UnityEngine;

public class HealthAbility : MonoBehaviour, IPoolable
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private GameObject exploseParticle;

    private ObjectPool pool;
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnDamage;

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Initialize(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamage?.Invoke();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Instantiate(exploseParticle, transform.position, transform.rotation);
        OnDeath?.Invoke();
        ReturnToPool();
    }

    public void SetPool(ObjectPool objectPool) => pool = objectPool;

    public void ReturnToPool()
    {
        OnDeath = null;
        OnHealthChanged = null;
        OnDamage = null;

        pool?.ReturnObject(gameObject);
    }
}