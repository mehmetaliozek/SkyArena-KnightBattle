using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    [HideInInspector] public Stats stats;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    private Rigidbody2D rgb;
    private Vector3 velocity;
    private float currentHealth;
    private float nextAttackTime;

    // private float hit = 0.3f;

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
        // Hit();
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
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
        else if (!PlayerAnimationEvents.instance.isDeath)
        {
            Death();
        }
    }

    // Güncelleme gelcek
    // private void Hit()
    // {
    //     if (isHit)
    //     {
    //         hit -= Time.deltaTime;
    //         if (hit > 0.15f)
    //         {
    //             GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
    //         }
    //         else if (hit > 0)
    //         {
    //             GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
    //         }
    //         else
    //         {
    //             hit = 0.3f;
    //             isHit = false;
    //         }
    //     }
    // }

    private void Death()
    {
        PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.death);
    }

    public void FallDamage()
    {
        PlayerAnimationEvents.instance.animator.SetTrigger(PlayerAnimationParametres.fall);
    }
}
