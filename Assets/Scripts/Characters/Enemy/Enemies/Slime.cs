using UnityEngine;

public class Slime : Enemy
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
        // Hedef birimle aralarında olan mesafe
        float distance = Vector2.Distance(transform.position, target.position);
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
        // Rastgele noktaya doğru hareket etme
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, stats.moveSpeed * Time.deltaTime);

        // Rastgele noktaya yakın bir konuma varınca bir süre bekleme
        if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
        {
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
        // Aşağıdaysa bu uzaklık belirttiğimiz durma uzaklığında büyükse oyuncuya yaklaşıyor değilse saldırıyor
        if (distance > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.moveSpeed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        currentAttackRate -= Time.deltaTime;

        if (currentAttackRate <= 0)
        {
            // Belirli bir yarıçapta saldırı yapacğı birim sayısı 0 değilse Oyuncuya hasar veriyor
            if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length != 0)
            {
                Player.instance.TakeDamage(stats.attack);
            }
            currentAttackRate = stats.attackRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.3f);
    }
}