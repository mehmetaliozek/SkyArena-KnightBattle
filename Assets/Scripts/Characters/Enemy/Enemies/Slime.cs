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
    
    protected new void FollowPlayer(float distance)
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

    protected new void Attack()
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