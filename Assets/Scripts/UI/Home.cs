using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour, IPointerEvents
{
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite up;
    private TextMeshProUGUI textMesh;

    public void PointerDown(GameObject btn)
    {
        btn.GetComponent<Image>().sprite = down;
        textMesh = btn.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.alignment = TextAlignmentOptions.Center;
    }

    public void PointerUp(GameObject btn)
    {
        btn.GetComponent<Image>().sprite = up;
        textMesh.alignment = TextAlignmentOptions.Top;

        switch (textMesh.text)
        {
            case ButtonText.play:
                Play();
                break;
            case ButtonText.options:
                Options();
                break;
            case ButtonText.exit:
                Exit();
                break;
        }
    }

    private void Play()
    {
        SceneManager.LoadScene(1);
    }

    private void Options()
    {
        SceneManager.LoadScene(2);
    }

    private void Exit()
    {
        Application.Quit();
    }
}