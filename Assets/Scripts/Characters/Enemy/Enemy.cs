using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Stats stats;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayers;
    [SerializeField] private float followingDistance;
    [SerializeField] private float stoppingDistance;
    private Animator animator;
    private Rigidbody2D rgb;
    private Vector2 moveSpot;
    private Transform target;
    private float waitTime;
    private bool canTakeDamage = true;
    private float currentHealth;
    private float currentAttackRate;

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
        if (canTakeDamage)
        {
            AI();
        }
        LookAtPlayer();
    }

    private void AI()
    {
        float distance = Vector2.Distance(transform.position, target.position);
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
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, stats.speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                moveSpot = EnemySpawner.instance.RandomPosition();
                waitTime = 1.5f;
            }
        }
    }

    private void FollowPlayer(float distance)
    {
        // Vector2.Distance fonksiyonu ile oyuncuya olan uzaklığı veriyor
        // Aşağıdaysa bu uzaklık belirttiğimiz durma uzaklığında büyükse oyuncuya yaklaşıyor
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
        // Oyuncu düşmanın sağında mı solunda mı diye kontrol edip düşanın yüzünü oyuncuya çeviriyor
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

    public void HurtEnd()
    {
        rgb.velocity = Vector2.zero;
    }
}