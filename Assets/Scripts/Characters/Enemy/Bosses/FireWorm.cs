using UnityEngine;

public class FireWorm : Enemy
{
  [SerializeField] private GameObject fireBall;
  [SerializeField] private GameObject unlem;
  [SerializeField] private float fireballtime=2f;

    private void Update() {
       Hurt();
       fireballtime-=Time.deltaTime;
       if(fireballtime<0.6f){
        unlem.SetActive(true);
       }
       if(fireballtime<0){
          fireballtime=2f;
          for(int i = 5;i>=1;i--){
                 SpawnFireBall();
                 unlem.SetActive(false);
          }
       }
       LookAtPlayer(target.position.x);
        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            
            AI();
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
    //Fireballı aldım
    private void SpawnFireBall()
    {
       
        
       
        Instantiate(fireBall, new Vector2(attackPoint.position.x,attackPoint.position.y), Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.5f);
    }
}