using TMPro;
using UnityEngine;

public class SpeechBubbles : MonoBehaviour
{
    [SerializeField] private GameObject speechBubbles;
    [SerializeField] private TextMeshProUGUI text;

    private string[] texts = new string[5] {
        "Pause? Really? I don't want to stop the fight.",
        "You are such a noob, git gud.",
        "Leaving the game? Are you scared? Get on the fucking game.",
        "You are gonna pause the game when there are still enemies? oh you better didn't.",
        "Berserker, Don't stop now. Shiour is still alive.",
        };

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