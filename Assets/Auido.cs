using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auido : MonoBehaviour
{
    // Start is called before the first frame update
    //Ses ve Müzik İçin game objectler
    public GameObject auido;
    public GameObject auido1;
    public GameObject auido2;
    //Slidera bağlı fonksiyon müzik için
    public void AuidoChange(float value){
        auido.GetComponent<AudioSource>().volume=value;
    }
    //Slidera bağlı fonkisyon ses efekti için
    public void AuidoChange1(float value){
        auido1.GetComponent<AudioSource>().volume=value;
    }
     public void AuidoChange2(float value){
        auido2.GetComponent<AudioSource>().volume=value;
    }
    //Ses Efektini oynatma fonksiyonu
    public void AuidoEffectPlay(){
        auido1.GetComponent<AudioSource>().Play();
    }
}
