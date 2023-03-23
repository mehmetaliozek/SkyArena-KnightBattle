using UnityEngine;

public class DoubleHead : Enemy
{
    [SerializeField] private float bodyAttackDistance;
    [SerializeField] private float bodyAttackCoolDown;
    [SerializeField] private float bodyAttackMoveSpeed;
    private float currentBodyAttackCoolDown;
    private bool isBodyAttackActive = false;

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
        LookAtPlayer(target.position.x);
        float distance = Vector2.Distance(transform.position, target.position);
        animator.SetFloat(EnemyAnimationParametres.velocity, 1);
        // Mesafe takip mesafesinden küçükse oyuncu takip edilcek değilse rastgele yürüycek
        if (distance < followingDistance)
        {
            if (currentBodyAttackCoolDown >= 0)
            {
                FollowPlayer(distance);
                if (distance > bodyAttackDistance)
                {
                    currentBodyAttackCoolDown -= Time.deltaTime;
                }
                else
                {
                    currentBodyAttackCoolDown = bodyAttackCoolDown;
                }
            }
            else
            {
                BodyAttack(distance);
            }
        }

        Hurt();
    }

    protected override void Patrol()
    {

    }

    protected override void FollowPlayer(float distance)
    {
        if (distance > stoppingDistance && canAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.moveSpeed * Time.deltaTime);
        }
        else if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length == 0 && transform.position != newTarget.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, newTarget.position, stats.moveSpeed * Time.deltaTime);
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
            animator.SetFloat(EnemyAnimationParametres.attackIndex, Random.Range(0, 2));
            canAttack = false;
        }
    }

    private void BodyAttack(float distance)
    {
        if (!isBodyAttackActive)
        {
            animator.SetTrigger(EnemyAnimationParametres.attack);
            animator.SetFloat(EnemyAnimationParametres.attackIndex, 2);
            isBodyAttackActive = true;
        }

        if (distance > stoppingDistance / 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.moveSpeed * bodyAttackMoveSpeed * Time.deltaTime);
        }
        else
        {
            if (Physics2D.OverlapCircleAll(transform.position, stats.attackRange, playerLayers).Length != 0)
            {
                Player.instance.TakeDamage(stats.attack * 2.5f);
            }

            currentBodyAttackCoolDown = bodyAttackCoolDown;
            isBodyAttackActive = false;
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

    private void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            stats.currentHealth -= (damage - (damage * stats.defense));
            if (stats.currentHealth <= 0)
            {
                // Düşmanın canı sıfırın altına inince hasar yemesini ve animasyonunun tekrar tekrar tetiklenmesini önlemek için hasar alabilirliğini konrol ediyoz
                canTakeDamage = !canTakeDamage;
                // Ölüm animasyonunu tetikliyor
                animator.SetTrigger(EnemyAnimationParametres.death);
            }
            Debug.Log(stats.currentHealth);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.3f);
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
}