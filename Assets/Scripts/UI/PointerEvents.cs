using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointerEvents : MonoBehaviour
{
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite up;

    public void PointerDown()
    {
        GetComponent<Image>().sprite = down;
        GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
    }

    public void PointerUp()
    {
        GetComponent<Image>().sprite = up;
        GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Top;
    }
}