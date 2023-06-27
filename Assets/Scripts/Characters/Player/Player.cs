using UnityEngine;

public class Player : MonoBehaviour
{
    // Sahnede bulunan Player scripti
    public static Player instance;

    public GameObject victory;
    public GameObject defeat;

    // public GameObject AudioManager;

    // Statların tutulduğu değişken
    [HideInInspector] public Stats stats;

    // Saldırının gerçekleştirilceği konumun merkezi
    [SerializeField] private Transform attackPoint;

    // Saldırının uygulancağı layer
    [SerializeField] private LayerMask enemyLayers;

    // Oyuncunun can barı
    [SerializeField] private HealtBar healtBar;
    [SerializeField] private FixedJoystick joystick;

    // Oyuncunun rigidbodysi
    private Rigidbody2D rgb;

    // Oyuncu yürüken verilen hız vektöru
    private Vector3 velocity;

    // Oyuncunun anlık saldırı hızı
    private float nextAttackTime;
    public GameObject SlimeEffect;//slime effect tutmak için
    public GameObject FireEffect;// fire effect tutmak için

    public bool attack = false;
    public bool dash = false;

    private bool isFall = false;
    private float fallTime = 2f;


    // İlk değer atamaları
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        rgb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        stats.currentHealth = stats.maxHealth;
        healtBar.SetMaxHealth(stats.maxHealth);
    }

    private void Update()
    {
        Move();
        if (isFall)
        {
            fallTime -= Time.deltaTime;
            if (fallTime <= 0)
            {
                Time.timeScale = 0;
                healtBar.SetHealth(0);
                defeat.SetActive(true);
            }
        }
    }

    private void Move()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        //Karakter hareketi eklendi ve speed ile hızı kontrol ediliyor
        if (PlayerAnimationEvents.instance.canMove)
        {
            velocity = new Vector3(x, y, 0f);
            transform.position += velocity * stats.moveSpeed * Time.deltaTime;
        }

        //Animasyon kontrolu
        if (Mathf.Abs(x) > 0.1)
        {
            PlayerAnimationEvents.instance.animator.SetFloat(PlayerAnimationParametres.velocity, Mathf.Abs(joystick.Horizontal));
        }
        else if (Mathf.Abs(y) > 0.1)
        {
            PlayerAnimationEvents.instance.animator.SetFloat(PlayerAnimationParametres.velocity, Mathf.Abs(joystick.Vertical));
        }
        else
        {
            PlayerAnimationEvents.instance.animator.SetFloat(PlayerAnimationParametres.velocity, 0);
        }

        if (!PlayerAnimationEvents.instance.isDeath)
        {
            //Karakterin dönmesi

            if (joystick.Horizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (joystick.Horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    public void Attack()
    {
        if (PlayerAnimationEvents.instance.canAttack && Time.time >= nextAttackTime)
        {

            PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.attack);

            // Beli bir yarıçapta Enemy layerına sahip nesneleri topluyoruz
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                // Topladığımız neslerin hepsinde TakeDamage fonskiyonunu çağırıp hasar almalarını sağlıyoz
                // Enemyden miras almış class TakeDamage fonksiyonunu çalıştırması için mesaj atıyoz
                enemy.SendMessage(EnemyFunctions.takeDamage, stats.attack);
            }

            nextAttackTime = Time.time + stats.attackRate;
        }
    }

    public void Roll()
    {
        if (PlayerAnimationEvents.instance.canRoll)
        {
            PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.roll);
        }
    }

    public void TakeDamage(float damage)
    {
        if (PlayerAnimationEvents.instance.canTakeDamage)
        {
            // Anlık canımız sıfırın üzerinde olduğu sürece canımız azalıyor ve hasar animasyonu çalışıyor
            if (stats.currentHealth > 0)
            {
                stats.currentHealth -= (damage - (damage * stats.defense));
                PlayerAnimationEvents.instance.isHurt = true;
                healtBar.SetHealth(stats.currentHealth);
            }
            else if (!PlayerAnimationEvents.instance.isDeath)
            {
                Death();
            }
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
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        isFall = true;
        PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.fall);
    }

    public void UpdateHealtBar()
    {
        healtBar.UpdateHealtBar(stats.maxHealth, stats.currentHealth);
    }
}
