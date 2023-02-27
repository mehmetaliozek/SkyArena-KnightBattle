using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats;
    [SerializeField] private float stoppingDistance;
    private Transform target;

    private void Start()
    {
        target = Player.instance.GetComponent<Transform>();
        stats = GetComponent<Stats>();
    }

    private void Update()
    {
        PlayerFollow();
    }

    private void PlayerFollow()
    {
        // Vector2.Distance fonksiyonu ile oyuncuya olan uzaklığı veriyor
        // Aşağıdaysa bu uzaklık belirttiğimiz durma uzaklığında büyükse oyuncuya yaklaşıyor
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.speed * Time.deltaTime);
        }

        if (transform.position.x - target.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void TakeDamage(float damage)
    {
        stats.health -= damage;
        if (stats.health <= 0)
        {
            WaveManager.instance.aliveEnemyCount--;
            Destroy(gameObject, 0.1f);
        }
    }

    public void FallDamage()
    {
        if (GetComponent<SpriteRenderer>().sortingLayerName == "Default")
        {
            // Düşman yok edildiğinde
            WaveManager.instance.aliveEnemyCount--;
            Destroy(gameObject, 2.0f);
        }
    }
}