using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private Image healthBar;
    [SerializeField] private int health;
    [SerializeField] public int gold;
    [SerializeField] private float speed;

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
            healthBar.fillAmount = health / 100f;
            if (health<=0)
            {
                OnDie();   
            }
        }
    }

    private void OnEnable()
    {
        this.Health = 100;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Ended) return;
        if (!targetLocation) return;
        Vector3 dir = targetLocation.position - transform.position;
        if (dir.magnitude<=1f) return;
        transform.Translate(dir.normalized * (this.speed * Time.deltaTime), Space.World);
    }

    public void TakeDamage(int amount)
    {
        this.Health -= amount;
    }
    
    public void SetTarget(Transform target)
    {
        this.targetLocation = target;
    }

    public void OnDie()
    {
        EnemyManager.Instance.DestroyEnemy(this.gameObject);
    }
}

