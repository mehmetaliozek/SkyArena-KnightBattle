using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    // Hayattaki düşman sayısı
    [HideInInspector] public int aliveEnemyCount = 0;

    // Bulunduğumuz dalga
    private int currentWave = 0;

    // Bulunduğumuz dalganın alt dalgası
    private int currentSubwave = 0;

    // Dalgaların kaç tane alt dalgası olcağını ve kaç düşman çıkcağını tutan dizi
    private int[,] waveInfo = new int[20, 2]{
        {5,1},
        {6,1},
        {7,1},
        {5,2},
        {6,2},
        {7,2},
        {5,3},
        {6,3},
        {7,3},
        {1,1},

        {5,4},
        {6,4},
        {7,4},
        {5,5},
        {6,5},
        {7,5},
        {5,6},
        {6,6},
        {7,6},
        {1,1},
    };

    private int[,] enemyIndexes = new int[10, 2]{
        {0,1},
        {0,2},
        {0,3},
        {2,4},
        {2,5},
        {3,5},
        {3,6},
        {4,7},
        {5,8},
        {8,9},
    };

    private int[] x = new int[2] { 0, 0 };

    // Dalga aralarında geçen süre
    private float time = 0;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        UpdateEnemyCount();
        x[0] = enemyIndexes[currentWave, 0];
        x[1] = enemyIndexes[currentWave, 1];
        EnemySpawner.instance.SpawnEnemy(waveInfo[currentWave, 0], x);
    }

    private void Update()
    {
        // Bulunduğumuz dalgadaki düşman sayısı sıfırlanınca yeni dalgaya geçme işlemleri
        if (aliveEnemyCount == 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                // Alt dalgalar tamamlandığında yeni bir ana dalgaya geçmek için currentWave'i arttırıyoruz
                // Yeni dalgaya geçince alt dalgayı sıfırlıyoruz
                currentSubwave++;
                if (currentSubwave == waveInfo[currentWave, 1])
                {
                    currentWave++;
                    currentSubwave = 0;
                    Debug.Log("Dalga: " + (currentWave + 1));
                }
                UpdateEnemyCount();
                // Yeni dalgaya geçtiğimiz için yaratılan düşman sayısını sıfırlıyoruz
                EnemySpawner.instance.createdEnemyCount = 0;
                // Düşman oluşturma fonksiyonu

                x[0] = enemyIndexes[currentWave, 0];
                x[1] = enemyIndexes[currentWave, 1];
                EnemySpawner.instance.SpawnEnemy(waveInfo[currentWave, 0], x);
                time = 2;
            }
        }
    }

    // Bulunduğumuz dalgada olmadı gereken düşman sayısını güncelliyor
    public void UpdateEnemyCount()
    {
        aliveEnemyCount = waveInfo[currentWave, 0];
    }

}