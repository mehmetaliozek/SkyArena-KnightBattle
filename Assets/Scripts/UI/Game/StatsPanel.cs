using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts;
    private void OnEnable()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = Player.instance.stats.GetStats(i).ToString();
        }
    }
}