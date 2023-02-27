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

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        rgb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Move(x, y);
        Attack();
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
        if (Input.GetMouseButtonDown(0))
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
    }

    private void AttackEnd()
    {
        move = true;
    }

    public void FallDamage()
    {
        animator.SetTrigger(PlayerAnimationParametres.fall);
    }
}
