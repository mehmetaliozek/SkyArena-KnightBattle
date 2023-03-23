using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Statların tutulduğu değişken
    protected Stats stats;

    // Saldırının gerçekleştirilceği konumun merkezi
    [SerializeField] protected Transform attackPoint;

    // Saldırının uygulancağı layer
    [SerializeField] protected LayerMask playerLayers;

    // Oyuncuyu takip etmeye başlıycağı mesafe
    [SerializeField] protected float followingDistance;

    // Oyuncuya en fazla yaklaşabilceği mesafe
    [SerializeField] protected float stoppingDistance;
    [SerializeField] protected bool hurtAnimation;

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


    // İlk değer atamaları
    private void Start()
    {
        animator = GetComponent<Animator>();
        rgb = GetComponent<Rigidbody2D>();
        moveSpot = EnemySpawner.instance.RandomPosition();
        target = Player.instance.transform;
        newTarget = Player.instance.GetComponentsInChildren<Transform>()[1];
        stats = GetComponent<Stats>();
        stats.currentHealth = stats.maxHealth;
        currentAttackRate = stats.attackRate;
    }

    protected abstract void AI();

    protected abstract void Patrol();

    protected abstract void FollowPlayer(float distance);

    protected abstract void Attack();

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
            if (stats.currentHealth <= 0)
            {
                // Düşmanın canı sıfırın altına inince hasar yemesini ve animasyonunun tekrar tekrar tetiklenmesini önlemek için hasar alabilirliğini konrol ediyoz
                canTakeDamage = !canTakeDamage;
                // Ölüm animasyonunu tetikliyor
                animator.SetTrigger(EnemyAnimationParametres.death);
            }
            else
            {
                Vector3 direction = transform.position - target.position;
                // Düşman birimi dinamik yani kuvvetlerden etkilenebilir bi hale getiriyoz
                rgb.isKinematic = false;
                // Açısal olarak kuvvet uygulamamıza sağlıyor
                rgb.AddForceAtPosition(direction.normalized * 50, target.position);

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
        WaveManager.instance.aliveEnemyCount--;
        Destroy(gameObject, 0.1f);
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