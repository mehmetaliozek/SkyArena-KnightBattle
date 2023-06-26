using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointerEvents : MonoBehaviour
{
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite up;

    [SerializeField] private RectTransform image;

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

    public void ImageButtonDown()
    {
        image.anchoredPosition = new Vector2(0, 12.5f);
    }

    public void ImageButtonUp()
    {
        image.anchoredPosition = new Vector2(0, 17.5f);
    }
}