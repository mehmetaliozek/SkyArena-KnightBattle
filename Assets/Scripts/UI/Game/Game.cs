using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private Audio audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    //         pauseMenu.SetActive(Time.timeScale == 0 ? true : false);
    //     }
    // }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        audioManager.playAudioEffect();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        audioManager.playAudioEffect();
    }

    public void Exit(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        audioManager.playAudioEffect();
    }
}