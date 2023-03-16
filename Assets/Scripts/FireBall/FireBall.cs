using UnityEngine;

public class FireBall : MonoBehaviour
{
    // Fire Ball animator
    [SerializeField] private Animator animator;
    // Fire Ball un hasar verceği layer
    [HideInInspector] public LayerMask playerLayers;
    // Fire Ball un hasar gücü
    [HideInInspector] public float attack;
    // Fire Ball un gideceği konum
    private Vector3 target;
    // Fire Ball un o konuma gidiş hızı
    private float speed = 5f;
    // Fire Ball un bakacağı konum
    private Vector3 lookDir;
    // Fire Ball un o konuma gidiş açısı
    private float angle;
    // Fire Ball un patlama durumu
    private bool isExplosion = false;

    private bool isCollision = false;

    private void Start()
    {
        // Fire Ball un gideceği konum ayarlanıyor
        target = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y);
        lookDir = target - transform.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, angle);
    }

    private void Update()
    {
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
        Debug.Log("coll");
        if (other.tag == Tags.player)
        {
            isCollision = true;
        }
    }

    // Patlama esnasında menzilde Oyuncu varsa oyuncuya hasar veriyor
    private void Explosion()
    {
        if (Physics2D.OverlapCircleAll(transform.position, 0.3f, playerLayers).Length != 0)
        {
            Player.instance.TakeDamage(attack);
        }
    }

    // Patlamadan sonra Fire Ball yok oluyor
    private void Death()
    {
        Destroy(gameObject, 0.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}