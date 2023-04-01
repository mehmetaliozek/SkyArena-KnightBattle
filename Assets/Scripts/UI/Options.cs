using UnityEngine;

public class Options : MonoBehaviour
{  
     public GameObject gameObject;      
     

     private void Update() {
          
     }
     //Ayarlar sekmesini açma kapama
     public void OffOptions(){
      gameObject.SetActive(false);
     }
     public void OnOptions(){
      gameObject.SetActive(true);
     }

     public void FullScreen(bool isfull){
        Screen.fullScreen=isfull;
     }
     
}