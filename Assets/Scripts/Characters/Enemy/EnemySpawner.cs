using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [SerializeField] private GameObject enemy;
    // Düşamların üzerinde spawnlancağı bölgenin collideri
    [SerializeField] private PolygonCollider2D spawnArea;
    private bool isRunning = true;
    [HideInInspector] public int createdEnemyCount = 0;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SpawnEnemy(int enemyCount)
    {
        while (createdEnemyCount != enemyCount)
        {
            Instantiate(enemy, RandomPosition(), Quaternion.identity);
            createdEnemyCount++;
        }
    }

    public Vector2 RandomPosition()
    {
        Vector2 spawnPosition = new Vector2();      
        while (true)
        {
            // Düşmanların spawnlancağı konumun x ve y sini random atıyoruz
            float x = Random.Range(-10, 10);
            float y = Random.Range(-10, 10);

            // ColsestPoint fonksiyonu rastgele oluşturduğumuz spawnlanma konumunun spawnArea ya en yakın noktasını döndürür
            // Bu nokta rastgele oluşan konumla aynı ise spawnArea nın üzerinde olduğumuz anlamına gelir
            spawnPosition = spawnArea.ClosestPoint(new Vector2(x, y));
            if (spawnPosition.x == x && spawnPosition.y == y)
            {
                break;
            }
        }

        return spawnPosition;
    }
}