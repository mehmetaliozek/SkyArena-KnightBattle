using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cloud;
    [SerializeField] private Sprite[] cloudSprites;

    private float countdown = 0;
    private const float top = 0.0f;
    private const float down = -5.0f;

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
        cloud.GetComponent<SpriteRenderer>().sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        Instantiate(cloud, new Vector3(transform.position.x, Random.Range(top, down), 0), Quaternion.identity);
    }

}
