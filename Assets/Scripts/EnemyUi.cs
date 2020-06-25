using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EnemyUi : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Enemy enemy;
    
    private void Start()
    {
        enemy = this.GetComponent<Enemy>();
        enemy.HealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        enemy.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int health)
    {
        this.healthBar.fillAmount = (health / 100f);
    }
}