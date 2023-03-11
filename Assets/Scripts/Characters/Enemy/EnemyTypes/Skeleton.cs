using UnityEngine;

public class Skeleton : Enemy
{
    private void Update()
    {
        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            AI();
        }
    }

    protected override void AI()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        animator.SetFloat(EnemyAnimationParametres.velocity, 1);
        // Mesafe takip mesafesinden küçükse oyuncu takip edilcek değilse rastgele yürüycek
        if (distance < followingDistance)
        {
            FollowPlayer(distance);
            LookAtPlayer(target.position.x);
        }
        else
        {
            Patrol();
            LookAtPlayer(moveSpot.x);
        }
    }

    protected override void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, stats.speed * Time.deltaTime);

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

    protected override void FollowPlayer(float distance)
    {
        if (distance > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.speed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        animator.SetFloat(EnemyAnimationParametres.velocity, 0);
        currentAttackRate -= Time.deltaTime;

        if (currentAttackRate <= 0 && canAttack)
        {
            animator.SetTrigger(EnemyAnimationParametres.attack);
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.5f);
    }
}