using UnityEngine;

public class HellBeast : Enemy
{
    [SerializeField] private GameObject fireBall;
    private float teleportationTime = 0;
    private bool playerInFireTornado = false;
    private void Update()
    {
        LookAtPlayer(target.position.x);
        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            AI();
        }

        if (playerInFireTornado)
        {
            Player.instance.TakeDamage(stats.attack / 100f);
        }

    }

    protected override void AI()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        // Mesafe takip mesafesinden küçükse oyuncu takip edilcek değilse rastgele yürüycek
        if (distance < followingDistance)
        {
            Attack();
        }
        Patrol();
    }

    protected override void Attack()
    {
        currentAttackRate -= Time.deltaTime;

        if (currentAttackRate <= 0)
        {
            animator.SetTrigger(EnemyAnimationParametres.attack);
            currentAttackRate = stats.attackRate;
        }
    }

    protected override void FollowPlayer(float distance)
    {
        // Hell Beast ışınlandığından oyuncuyu takip etmesine gerek yok
    }

    protected override void Patrol()
    {
        teleportationTime -= Time.deltaTime;
        if (teleportationTime <= 0)
        {
            moveSpot = EnemySpawner.instance.RandomPosition();
            transform.position = moveSpot;
            teleportationTime = stats.moveSpeed;
        }
    }

    private void SpawnFireBall()
    {
        fireBall.GetComponent<FireBall>().attack = stats.attack;
        fireBall.GetComponent<FireBall>().playerLayers = playerLayers;
        Instantiate(fireBall, attackPoint.position, Quaternion.identity);
    }

    private void EnableBoxCollider2D()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            playerInFireTornado = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            playerInFireTornado = false;
        }
    }
}