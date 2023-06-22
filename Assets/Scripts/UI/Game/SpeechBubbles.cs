using TMPro;
using UnityEngine;

public class SpeechBubbles : MonoBehaviour
{
    [SerializeField] private GameObject speechBubbles;
    [SerializeField] private TextMeshProUGUI text;

    //TODO:mantılı cümleler kur
    private string[] texts = new string[10] { "a", "b", "c", "ç", "d", "e", "f", "g", "ğ", "h" };

    private void OnEnable()
    {
        if (Random.Range(0, 100) < 25)
        {
            text.text = texts[Random.Range(0, texts.Length)];
            speechBubbles.SetActive(true);
        }
    }

    private void OnDisable()
    {
        speechBubbles.SetActive(false);
    }
}