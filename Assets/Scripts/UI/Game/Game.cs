using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            pauseMenu.SetActive(Time.timeScale == 0 ? true : false);
        }
    }
}