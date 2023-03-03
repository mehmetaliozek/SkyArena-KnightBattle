using UnityEngine;

public class Player : MonoBehaviour
{
    // Sahnede bulunan Player scripti
    public static Player instance;

    // Statların tutulduğu değişken
    [HideInInspector] public Stats stats;

    // Saldırının gerçekleştirilceği konumun merkezi
    [SerializeField] private Transform attackPoint;

    // Saldırının uygulancağı layer
    [SerializeField] private LayerMask enemyLayers;

    // Oyuncunun rigidbodysi
    private Rigidbody2D rgb;

    // Oyuncu yürüken verilen hız vektöru
    private Vector3 velocity;

    // Oyuncunun anlık canı
    private float currentHealth;

    // Oyuncunun anlık saldırı hızı
    private float nextAttackTime;

    // İlk değer atamaları
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        rgb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        currentHealth = stats.maxHealth;
    }

    private void Update()
    {
        Move();
        Attack();
        Roll();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Karakter hareketi eklendi ve speed ile hızı kontrol ediliyor
        if (PlayerAnimationEvents.instance.canMove)
        {
            velocity = new Vector3(x, y, 0f);
            transform.position += velocity * stats.speed * Time.deltaTime;
        }

        //Animasyon kontrolu
        if (Mathf.Abs(x) > 0)
        {
            PlayerAnimationEvents.instance.animator.SetFloat(PlayerAnimationParametres.velocity, Mathf.Abs(Input.GetAxis("Horizontal")));
        }
        else if (Mathf.Abs(y) > 0)
        {
            PlayerAnimationEvents.instance.animator.SetFloat(PlayerAnimationParametres.velocity, Mathf.Abs(Input.GetAxis("Vertical")));
        }

        if (!PlayerAnimationEvents.instance.isDeath)
        {
            //Karakterin dönmesi
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && PlayerAnimationEvents.instance.canAttack && Time.time >= nextAttackTime)
        {
            PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.attack);

            // Beli bir yarıçapta Enemy layerına sahip nesneleri topluyoruz
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                // Topladığımız neslerin hepsinde TakeDamage fonskiyonunu çağırıp hasar almalarını sağlıyoz
                enemy.GetComponent<Enemy>().TakeDamage(stats.attack);
            }
            nextAttackTime = Time.time + stats.attackRate;
        }
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerAnimationEvents.instance.canRoll)
        {
            PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.roll);
        }
    }

    public void TakeDamage(float damage)
    {
        // Anlık canımız sıfırın üzerinde olduğu sürece canımız azalıyor ve hasar animasyonu çalışıyor
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            PlayerAnimationEvents.instance.isHurt = true;
        }
        else if (!PlayerAnimationEvents.instance.isDeath)
        {
            Death();
        }
    }

    // Ölüm animasyonunu çalıştırıyor
    private void Death()
    {
        PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.death);
    }

    // Düşüş animasyonunu çalıştırıyor
    public void FallDamage()
    {
        PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.fall);
    }
}
