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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.5f);
    }
}