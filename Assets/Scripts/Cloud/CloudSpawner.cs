using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cloud;
    [SerializeField] private Sprite[] cloudSprites;

    private float countdown = 0;
    private const float top = 5.0f;
    private const float down = -5.0f;

    // 3.75 saniyede bir bulut oluşturuyoz
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Spawn();
            countdown = 3.75f;
        }
    }

    private void Spawn()
    {
        // Elimizdeki bulut spritelarından rastgele birini seçip buluta ekliyoz
        cloud.GetComponent<SpriteRenderer>().sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        // Bulutu rastgele bi noktada oluşturuyor
        Instantiate(cloud, new Vector3(transform.position.x, Random.Range(top, down), 0), Quaternion.identity);
    }

}
