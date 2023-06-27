using UnityEngine;

public class FireBall : MonoBehaviour
{
    // Fire Ball animator
    [SerializeField] private Animator animator;
    // Fire Ballun hasar verceği layer
    [HideInInspector] public LayerMask playerLayers;
    // Fire Ballun hasar gücü
    [HideInInspector] public float attack;
    // Fire Ballun gideceği konum
    private Vector3 target;
    // Fire Ballun o konuma gidiş hızı
    public float speed = 5f;
    // Fire Ballun bakacağı konum
    private Vector3 lookDir;
    // Fire Ballun o konuma gidiş açısı
    private float angle;
    // Fire Ballun patlama hasarının hissedildiği yarıçap
    private float radius = 0.3f;
    // Fire Ballun patlama durumu
    private bool isExplosion = false;
    // Fire Ballun oyuncuya erken çarpışma durumunu kontrol ediyor
    private bool isCollision = false;
    [HideInInspector] public bool isDamage = false;
    public GameObject PlayerFireEffect;
    public float FireffectTime = 2f;
    private void Start()
    {
        // Fire Ballun gideceği konum ayarlanıyor
        PlayerFireEffect = Player.instance.FireEffect;
        target = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y);
        lookDir = target - transform.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, angle);
    }

    private void Update()
    {
        if (isDamage == true)
        {
            Player.instance.TakeDamage(0.001f);
            FireffectTime -= Time.deltaTime;
            if (FireffectTime <= 0)
            {
                PlayerFireEffect.SetActive(false);
                Destroy(gameObject, 0.1f);
                isDamage = false;
                FireffectTime = 2f;
            }
        }
        else
        {
            FireffectTime = 2f;
        }
        // Konuma 0.1 birim yaklaşınca Fire Ball patlıyor
        if (Vector2.Distance(transform.position, target) > 0.1 && !isCollision)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else if (!isExplosion)
        {
            animator.SetTrigger(EnemyAnimationParametres.death);
            isExplosion = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            isCollision = true;
        }
    }

    // Patlama esnasında menzilde Oyuncu varsa oyuncuya hasar veriyor
    private void Explosion()
    {
        if (Physics2D.OverlapCircleAll(transform.position, radius, playerLayers).Length != 0)
        {
            if (PlayerAnimationEvents.instance.canTakeDamage)
            {
                isDamage = true;
                PlayerFireEffect.SetActive(true);
                Player.instance.TakeDamage(attack);
            }
        }
    }

    // Patlamadan sonra Fire Ball yok oluyor
    private void Death()
    {
        if (isDamage == true)
        {
            gameObject.transform.position = new Vector2(1000, 1000);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}