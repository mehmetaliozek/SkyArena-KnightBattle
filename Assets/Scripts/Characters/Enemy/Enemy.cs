using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Statların tutulduğu değişken
    [HideInInspector] public Stats stats;

    // Saldırının gerçekleştirilceği konumun merkezi
    [SerializeField] private Transform attackPoint;

    // Saldırının uygulancağı layer
    [SerializeField] private LayerMask playerLayers;

    // Oyuncuyu takip etmeye başlıycağı mesafe
    [SerializeField] private float followingDistance;

    // Oyuncuya en fazla yaklaşabilceği mesafe
    [SerializeField] private float stoppingDistance;

    // Düşmanın animatoru
    private Animator animator;

    // Düşmanın rigidbodysi
    private Rigidbody2D rgb;

    // Oyuncu takip mesafesinde değilken gitceği nokta
    private Vector2 moveSpot;

    // Düşmanın takip edeceği birim
    private Transform target;

    // Rastgele bi noktaya gittikten sonra bekliyceği süre
    private float waitTime = 1.5f;

    // Rastgele bi noktaya gittikten sonra bekliyceği anlık süre
    private float currentWaitTime;

    // Düşmanın hasar alabilrliğini kontrol eden değişken
    private bool canTakeDamage = true;

    // Düşmanın anlık canı
    private float currentHealth;

    // Düşmanın anlık saldırı hızı
    private float currentAttackRate;

    // İlk değer atamaları
    private void Start()
    {
        animator = GetComponent<Animator>();
        rgb = GetComponent<Rigidbody2D>();
        moveSpot = EnemySpawner.instance.RandomPosition();
        target = Player.instance.GetComponent<Transform>();
        stats = GetComponent<Stats>();
        currentHealth = stats.maxHealth;
        currentAttackRate = stats.attackRate;
    }

    private void Update()
    {
        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            AI();
        }
        // Oyuncuya dönme
        LookAtPlayer();
    }

    private void AI()
    {
        // Hedef birimle aralarında olan mesafe
        float distance = Vector2.Distance(transform.position, target.position);
        // Mesafe takip mesafesinden küçükse oyuncu takip edilcek değilse rastgele yürüycek
        if (distance < followingDistance)
        {
            FollowPlayer(distance);
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Rastgele noktaya doğru hareket etme
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, stats.speed * Time.deltaTime);

        // Rastgele noktaya yakın bir konuma varınca bir süre bekleme
        if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
        {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0)
            {
                // Yeni bir rastgele nokta belirleme
                moveSpot = EnemySpawner.instance.RandomPosition();
                currentWaitTime = waitTime;
            }
        }
    }

    private void FollowPlayer(float distance)
    {
        // Aşağıdaysa bu uzaklık belirttiğimiz durma uzaklığında büyükse oyuncuya yaklaşıyor değilse saldırıyor
        if (distance > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.speed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }

    private void LookAtPlayer()
    {
        // Oyuncu düşmanın sağında mı solunda mı diye kontrol edip düşmanın yüzünü oyuncuya çeviriyor
        if (transform.position.x - target.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void Attack()
    {
        currentAttackRate -= Time.deltaTime;

        if (currentAttackRate <= 0)
        {
            // Belirli bir yarıçapta saldırı yapacğı birim sayısı 0 değilse Oyuncuya hasar veriyor
            if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length != 0)
            {
                Player.instance.TakeDamage(stats.attack);
            }
            currentAttackRate = stats.attackRate;
        }
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                // Düşmanın canı sıfırın altına inince hasar yemesini ve animasyonunun tekrar tekrar tetiklenmesini önlemek için hasar alabilirliğini konrol ediyoz
                canTakeDamage = !canTakeDamage;
                // Ölüm animasyonunu tetikliyor
                animator.SetTrigger(EnemyAnimationParametres.death);
            }
            else
            {
                Vector3 direction = transform.position - target.position;
                // Açısal olarak kuvvet uygulamamıza sağlıyor
                rgb.AddForceAtPosition(direction.normalized * 100, target.position);
                // Hasar animasyonunu tetikliyor
                animator.SetTrigger(EnemyAnimationParametres.hurt);
            }
        }
    }

    // Platformdan düştükten donra düşmanı yok etmeye yarayan fonksiyon
    public void FallDamage()
    {
        WaveManager.instance.aliveEnemyCount--;
        Destroy(gameObject, 2.0f);
    }

    // Düşman oyuncu tarafından öldürülürse animasyonun sonunda yok olmasını sağlıyor
    public void Death()
    {
        WaveManager.instance.aliveEnemyCount--;
        Destroy(gameObject, 0.1f);
    }

    // Hasar alındığındaki geri tepmeyi durduyor
    public void HurtEnd()
    {
        rgb.velocity = Vector2.zero;
    }
}