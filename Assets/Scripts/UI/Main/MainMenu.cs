using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject options;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Options(bool value)
    {
        options.SetActive(value);
    }

    public void Exit()
    {
        Application.Quit();
    }
}