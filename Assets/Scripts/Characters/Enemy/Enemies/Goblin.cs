using UnityEngine;

public class Goblin : Enemy
{
    [SerializeField] private GameObject bomb;

    private void Update()
    {
        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            AI();
        }
    }

    private void SpawnBomb()
    {
        bomb.GetComponent<Bomb>().attack = stats.attack * 3;
        bomb.GetComponent<Bomb>().playerLayers = playerLayers;
        Instantiate(bomb, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.7f);
    }
}