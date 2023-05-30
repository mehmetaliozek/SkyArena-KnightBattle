using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject go;

    private void Update()
    {

    }
    //Ayarlar sekmesini açma kapama
    public void OffOptions()
    {
        go.SetActive(false);
    }
    public void OnOptions()
    {
        go.SetActive(true);
    }

    public void FullScreen(bool isfull)
    {
        Screen.fullScreen = isfull;
    }

}