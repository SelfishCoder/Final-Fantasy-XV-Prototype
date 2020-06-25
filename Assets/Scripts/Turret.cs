using UnityEngine;

[DisallowMultipleComponent]
public class Turret : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private float fireRate = .5f;
    [SerializeField] private Enemy enemy = null;
    public int gold = 20;
    public int power = 25;
    public int level = 1;
    private float fireTimer = 1;
    
    public GameObject FindTarget()
    {
        Collider[] inArea = Physics.OverlapSphere(this.transform.position, range);
        float minDistance = Mathf.Infinity;
        GameObject target = null;
        foreach (var collider in inArea)
        {
            if (!collider.GetComponent<Enemy>()) continue;
            float distance = Vector3.Distance(this.transform.position, collider.transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                target = collider.gameObject;
            }
        }

        return target;
    }

    public void Shoot()
    {
        enemy.TakeDamage(this.power);
    }

    private bool TargetInRange(Vector3 targetposition)
    {
        return range >= Vector3.Distance(this.transform.position, enemy.transform.position);
    }
    
    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Ended) return;
        if (!enemy)
        {
            fireTimer = fireRate;
            GameObject enemy = FindTarget();
            if (!enemy)
            {
                return;
            } 
            Debug.Log("enemy");
            this.enemy = enemy.GetComponent<Enemy>();
        }

        fireTimer -= Time.deltaTime;
        if (TargetInRange(enemy.transform.position) && enemy.Health>0)
        {
            if (fireTimer<=0)
            {
                Shoot();
                fireTimer = fireRate;
            }
        }
        else
        {
            enemy = null;
        }
    }
}
