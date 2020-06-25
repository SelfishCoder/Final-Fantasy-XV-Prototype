using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] public int gold;
    public event Action<int> HealthChanged;
    
    public float Speed
    {
        get => speed;
        set => speed = value;
    }    
    
    public int Health
    {
        get => health;
        set
        {
            health = value;
            HealthChanged?.Invoke(health);
            if (health<=0)
            {
                Die();   
            }
        }
    }

    private void Start()
    {
        this.Health = 100;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Ended) return;
        if (!targetLocation) return;
        Vector3 direction = targetLocation.position - transform.position;
        if (direction.magnitude<=1f) return;
        transform.Translate(direction.normalized * (this.speed * Time.deltaTime), Space.World);
    }

    public void TakeDamage(int amount)
    {
        this.Health -= amount;
    }
    
    public void SetTarget(Transform target)
    {
        this.targetLocation = target;
    }

    private void Die()
    {
        EnemyManager.Instance.DestroyEnemy(this);
    }
}

