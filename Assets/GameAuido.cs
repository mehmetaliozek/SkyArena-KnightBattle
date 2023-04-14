using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAuido : MonoBehaviour
{
    public GameObject auido;//Müzik
    public GameObject auido2;//SesEfekti
    // Start is called before the first frame update
    public void AuidoChange(float value){
        auido.GetComponent<AudioSource>().volume=value;//Slider değerini ayarlama
    }
    public void AuidoChange2(float value){
        auido2.GetComponent<AudioSource>().volume=value;//Slider değerini ayarlama
    }
    public void SwordEffectPlay(){
        auido2.GetComponent<AudioSource>().Play();
    }

    
}
