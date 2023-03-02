using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Animator animator;
    [HideInInspector] public Stats stats;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    private Rigidbody2D rgb;
    private Vector3 velocity;
    private bool move = true;
    private bool attack = true;
    private bool roll = true;
    private bool isHit = false;

    private float currentHealth;
    private float hit = 0.3f;

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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Move(x, y);
        Attack();
        Roll();
        Hit();
    }

    private void Move(float x, float y)
    {
        //Karakter hareketi eklendi ve speed ile hızı kontrol ediliyor
        if (move)
        {
            velocity = new Vector3(x, y, 0f);
            transform.position += velocity * stats.speed * Time.deltaTime;
        }

        //Animasyon kontrolu
        if (Mathf.Abs(x) > 0)
        {
            animator.SetFloat(PlayerAnimationParametres.velocity, Mathf.Abs(Input.GetAxis("Horizontal")));
        }
        else if (Mathf.Abs(y) > 0)
        {
            animator.SetFloat(PlayerAnimationParametres.velocity, Mathf.Abs(Input.GetAxis("Vertical")));
        }

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

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && attack)
        {
            animator.SetTrigger(PlayerAnimationParametres.attack);

            // Beli bir yarıçapta Enemy layerına sahip nesneleri topluyoruz
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                // Topladığımız neslerin hepsinde TakeDamage fonskiyonunu çağırıp hasar almalarını sağlıyoz
                enemy.GetComponent<Enemy>().TakeDamage(stats.attack);
            }
        }
    }

    private void AttackStart()
    {
        move = false;
        attack = false;
    }

    private void AttackEnd()
    {
        move = true;
        attack = true;
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && roll)
        {
            animator.SetTrigger(PlayerAnimationParametres.roll);
        }
    }

    private void RollStart()
    {
        attack = false;
        roll = false;
    }

    private void RollEnd()
    {
        attack = true;
        roll = true;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth >= 0)
        {
            currentHealth -= damage;
            isHit = true;
            Death();
        }
    }

    // Güncelleme gelcek
    private void Hit()
    {
        if (isHit)
        {
            hit -= Time.deltaTime;
            if (hit > 0.15f)
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
            }
            else if (hit > 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
            }
            else
            {
                hit = 0.3f;
                isHit = false;
            }
        }
    }

    private void Death()
    {
        move = false;
        attack = false;
        roll = false;
        animator.SetTrigger(PlayerAnimationParametres.death);
    }

    private void DeathEnd()
    {
        animator.speed = 0;
    }

    public void FallDamage()
    {
        animator.SetTrigger(PlayerAnimationParametres.fall);
        attack = false;
        roll = false;
    }
}
