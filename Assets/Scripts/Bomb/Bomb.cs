using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Bombanın hasar verceği layer
    [HideInInspector] public LayerMask playerLayers;
    // Bombanın hasar gücü
    [HideInInspector] public float attack;
    // Bombanın patlama etkisinin hissedildiği yarıçap
    private float radius = 0.5f;
    // Bombanın patlama durumu
    private bool isExplosion = false;

    private void Update()
    {
        if (Physics2D.OverlapCircleAll(transform.position, radius, playerLayers).Length != 0 && isExplosion)
        {
            Player.instance.TakeDamage(attack);
            isExplosion = false;
        }
    }

    private void ExplosionStart()
    {
        isExplosion = true;
    }

    private void ExplosionEnd()
    {
        Destroy(gameObject, 0.1f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}