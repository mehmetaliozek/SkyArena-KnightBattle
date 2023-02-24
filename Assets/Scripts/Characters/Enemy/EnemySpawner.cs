using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    // Düşamların üzerinde spawnlancağı bölgenin collideri
    [SerializeField] private PolygonCollider2D spawnArea;
    private Vector2 spawnPosition;
    private bool isRunning = true;
    private float x;
    private float y;
    private int i = 0;

    private void Start()
    {

    }

    private void Update()
    {
        if (i != 5)
        {
            // Düşmanların spawnlancağı konumun x ve y sini random atıyoruz
            x = Random.Range(-10, 10);
            y = Random.Range(-10, 10);

            // ColsestPoint fonksiyonu rastgele oluşturduğumuz spawnlanma konumunun spawnArea ya en yakın noktasını döndürür
            // Bu nokta rastgele oluşan konumla aynı ise spawnArea nın üzerinde olduğumuz anlamına gelir
            spawnPosition = spawnArea.ClosestPoint(new Vector2(x, y));

            // Eğer konum aynı ise düşmanı oluşturuyoruz
            if (spawnPosition.x == x && spawnPosition.y == y)
            {
                Instantiate(enemy, spawnPosition, Quaternion.identity);
                i++;
            }
        }

    }
}