using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Statların tutulduğu değişken
    protected Stats stats;

    // Oyuncunun can barı
    [SerializeField] private HealtBar healtBar;

    // Saldırının gerçekleştirilceği konumun merkezi
    [SerializeField] protected Transform attackPoint;

    // Saldırının uygulancağı layer
    [SerializeField] protected LayerMask playerLayers;

    // Oyuncuyu takip etmeye başlıycağı mesafe
    [SerializeField] protected float followingDistance;

    // Oyuncuya en fazla yaklaşabilceği mesafe
    [SerializeField] protected float stoppingDistance;

    // Düşmanın hasar alma animasyonu varmı
    [SerializeField] protected bool hurtAnimation;

    // Düşmanın birden fazla saldırı animasyonu varmı
    [SerializeField] protected bool moreThanOneAttackAnimation;

    [SerializeField] private bool isBoss;

    // Düşmanın animatoru
    protected Animator animator;

    // Düşmanın rigidbodysi
    protected Rigidbody2D rgb;

    // Oyuncu takip mesafesinde değilken gitceği nokta
    protected Vector2 moveSpot;

    // Düşmanın takip edeceği birim
    protected Transform target;

    // Düşman oyuncunun yanındaysa ama oyuncu saldırı benzilinde değilse oyuncunun sağına veya soluna geçeceği 
    protected Transform newTarget;

    // Rastgele bi noktaya gittikten sonra bekliyceği süre
    protected float waitTime = 1.5f;

    // Rastgele bi noktaya gittikten sonra bekliyceği anlık süre
    protected float currentWaitTime;

    // Düşmanın hasar alabilrliğini kontrol eden değişken
    protected bool canTakeDamage = true;

    // Düşmanın anlık saldırı hızı
    protected float currentAttackRate;

    // Düşmanın saldırı yapailirliği
    protected bool canAttack = true;

    private bool isHurt = false;

    // Oyuncunun hasar alma durumunda yanıp sönmesini sağlyan değişkenler
    private float resetCount = 0;
    private float hurtTime = 0.1f;
    private float currentHurtTime;
    public GameObject PlayerSlowEffect;//yavaşlama efekti tutmak için
    public GameObject PlayerFireEffect;//ateş efekti tutmak için
    
    // İlk değer atamaları
    private void Start()
    {
        PlayerSlowEffect=Player.instance.SlimeEffect;//oyuncudan SlimeEffecti alıp oyuncuya verilecek Slime effecte atama yapıyorum
        PlayerFireEffect=Player.instance.FireEffect;//oyuncudan FireEffecti alıp oyuncuya verilecek fire effecte atama yapıyorum
        animator = GetComponent<Animator>();
        rgb = GetComponent<Rigidbody2D>();
        moveSpot = EnemySpawner.instance.RandomPosition();
        target = Player.instance.transform;
        newTarget = Player.instance.GetComponentsInChildren<Transform>()[1];
        stats = GetComponent<Stats>();
        stats.currentHealth = stats.maxHealth;
        currentAttackRate = stats.attackRate;
        healtBar.SetMaxHealth(stats.maxHealth);
    }

    protected void AI()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        animator.SetFloat(EnemyAnimationParametres.velocity, 1);
        // Mesafe takip mesafesinden küçükse oyuncu takip edilcek değilse rastgele yürüycek
        if (distance < followingDistance)
        {
            FollowPlayer(distance);
        }
        else
        {
            Patrol();
        }

        Hurt();
    }

    protected void Patrol()
    {
        LookAtPlayer(moveSpot.x);
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, stats.moveSpeed * Time.deltaTime);

        // Rastgele noktaya yakın bir konuma varınca bir süre bekleme
        if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
        {
            animator.SetFloat(EnemyAnimationParametres.velocity, 0);
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0)
            {
                // Yeni bir rastgele nokta belirleme
                moveSpot = EnemySpawner.instance.RandomPosition();
                currentWaitTime = waitTime;
            }
        }
    }

    protected void FollowPlayer(float distance)
    {
        LookAtPlayer(target.position.x);
        if (distance > stoppingDistance && canAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.moveSpeed * Time.deltaTime);
        }
        else if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length == 0 && transform.position != newTarget.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, newTarget.position, stats.moveSpeed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }

    protected void Attack()
    {
        animator.SetFloat(EnemyAnimationParametres.velocity, 0);
        currentAttackRate -= Time.deltaTime;

        if (currentAttackRate <= 0 && canAttack)
        {
            animator.SetTrigger(EnemyAnimationParametres.attack);
            if (moreThanOneAttackAnimation)
            {
                animator.SetFloat(EnemyAnimationParametres.attackIndex, Random.Range(0, 2));
            }
            canAttack = false;
        }
    }

    private void DealDamage()
    {
        // Belirli bir yarıçapta saldırı yapacğı birim sayısı 0 değilse Oyuncuya hasar veriyor
        if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length != 0)
        {
            Player.instance.TakeDamage(stats.attack);
        }
        currentAttackRate = stats.attackRate;
        canAttack = true;
    }

    private void FixAttack()
    {
        currentAttackRate = stats.attackRate;
        canAttack = true;
    }

    protected void LookAtPlayer(float x)
    {
        // Oyuncu düşmanın sağında mı solunda mı diye kontrol edip düşmanın yüzünü oyuncuya çeviriyor
        if (transform.position.x - x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            stats.currentHealth -= (damage - (damage * stats.defense));
            healtBar.SetHealth(stats.currentHealth);
            if (stats.currentHealth <= 0)
            {
                // Düşmanın canı sıfırın altına inince hasar yemesini ve animasyonunun tekrar tekrar tetiklenmesini önlemek için hasar alabilirliğini konrol ediyoz
                canTakeDamage = !canTakeDamage;
                // Ölüm animasyonunu tetikliyor
                animator.SetTrigger(EnemyAnimationParametres.death);
            }
            else
            {
                if (!isBoss)
                {
                    Vector3 direction = transform.position - target.position;
                    // Düşman birimi dinamik yani kuvvetlerden etkilenebilir bi hale getiriyoz
                    rgb.isKinematic = false;
                    // Açısal olarak kuvvet uygulamamıza sağlıyor
                    rgb.AddForceAtPosition(direction.normalized * 50, target.position);
                }

                if (hurtAnimation)
                {
                    // Hasar animasyonunu tetikliyor
                    animator.SetTrigger(EnemyAnimationParametres.hurt);
                }
                else
                {
                    // Hasar alınabilirliği aktif hale getiriyoruz
                    isHurt = true;
                }
            }
        }
    }

    // Platformdan düştükten donra düşmanı yok etmeye yarayan fonksiyon
    private void FallDamage()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        GetComponent<Collider2D>().enabled = false;
        rgb.gravityScale = 1.0f;
        rgb.isKinematic = false;
        WaveManager.instance.aliveEnemyCount--;
        Destroy(gameObject, 2.0f);
    }

    // Düşman oyuncu tarafından öldürülürse animasyonun sonunda yok olmasını sağlıyor
    private void Death()
    {
        Destroy(gameObject, 0.1f);
        WaveManager.instance.aliveEnemyCount--;
        PlayerSlowEffect.SetActive(false); 
    }

    protected void Hurt()
    {
        // Oyuncu hasar almış ise
        if (isHurt)
        {
            currentHurtTime -= Time.deltaTime;
            if (currentHurtTime <= 0)
            {
                switch (resetCount % 2)
                {
                    case 0:
                        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                        break;
                    case 1:
                        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                        break;
                }
                resetCount++;
                currentHurtTime = hurtTime;
                if (resetCount == 2)
                {
                    HurtEnd();
                }
            }
        }
    }

    // Hasar alındığındaki geri tepmeyi durduyor
    private void HurtEnd()
    {
        isHurt = false;
        resetCount = 0;
        rgb.velocity = Vector2.zero;
        rgb.isKinematic = true;
    }
}