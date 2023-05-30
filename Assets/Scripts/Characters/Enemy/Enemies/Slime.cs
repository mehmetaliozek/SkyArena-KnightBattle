using UnityEngine;

public class Slime : Enemy
{
    private int rndnumber;
    public float SlowEffectTime = 1.5f;
    private bool isSlowed = false;



    private void Update()
    {

        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            AI();
        }
        if (isSlowed)
        {

            SlowEffectTime -= Time.deltaTime;
            if (SlowEffectTime <= 0)
            {
                PlayerSlowEffect.SetActive(false);
                isSlowed = false;
                Player.instance.stats.moveSpeed = 2.5f;
                SlowEffectTime = 1.5f;
            }
        }
        else
        {
            SlowEffectTime = 1.5f;
        }
    }
    protected new void AI()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        // Mesafe takip mesafesinden küçükse oyuncu takip edilcek değilse rastgele yürüycek
        if (distance < followingDistance)
        {
            FollowPlayer(distance);
        }
        else
        {
            Patrol();
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

    float sloeTime = 1.5f;

    float currentSlowTime = 0;

    protected new void Attack()
    {
        currentAttackRate -= Time.deltaTime;

        if (currentAttackRate <= 0)
        {
            // Belirli bir yarıçapta saldırı yapacğı birim sayısı 0 değilse Oyuncuya hasar veriyor
            if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length != 0)
            {
                Player.instance.TakeDamage(stats.attack);
                //Yavaşlama Efekti İçin
                rndnumber = Random.Range(0, 2);
                if (rndnumber == 0)
                {
                    PlayerSlowEffect.SetActive(true);
                    isSlowed = true;
                    Player.instance.stats.moveSpeed = 1f;
                }

            }
            currentAttackRate = stats.attackRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.3f);
    }
}