using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats;
    [SerializeField] private float stoppingDistance;

    private Transform target;

    private bool isTakeDamage = false;

    private void Start()
    {
        target = Player.instance.GetComponent<Transform>();
        stats = GetComponent<Stats>();
    }

    private void Update()
    {
        PlayerFollow();
        TakeDamage();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Oyuncunun saldırı menziline girsiysek hasar alma özelliği açılıyor
        if (other.tag == Tags.playerAttack)
        {
            isTakeDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Oyuncunun saldırı menzilinden çıktığında hasar alma özelliği kapanıyor
        if (other.tag == Tags.playerAttack)
        {
            isTakeDamage = false;
        }
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

    private void TakeDamage()
    {
        // Hasar alma özelliği açık ve oyuncu mouse un sol tuşuna tıkladıysa düşman birim hasar alıyor
        if (isTakeDamage && Input.GetMouseButtonDown(0))
        {
            stats.health -= Player.instance.stats.attack;
            if (stats.health <= 0)
            {
                Destroy(gameObject, 0.5f);
            }
        }
    }
}