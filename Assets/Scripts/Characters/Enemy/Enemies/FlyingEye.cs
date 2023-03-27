using UnityEngine;

public class FlyingEye : Enemy
{
    private void Update()
    {
        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            AI();
        }
    }

    private void FallDamage()
    {
        // Uçan düşman olduğundan platformdan düşemez
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.2f);
    }
}