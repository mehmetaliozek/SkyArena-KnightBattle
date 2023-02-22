using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rgb;
    private float speed=2.5f;
    Vector3 velocity;
    float x,y;
    [SerializeField]private Animator animator;
    void Start()
    {
        rgb=GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("AttackTrigger");
           
        }
        
        x=Input.GetAxis("Horizontal");
        y=Input.GetAxis("Vertical");
        //Karakter hareketi eklendi ve speed ile hızı kontrol ediliyor
        velocity=new Vector3(x,y,0f);
        transform.position+=velocity*speed*Time.deltaTime;
        
        //Animasyon kontrolu
        if(Mathf.Abs(x)>0){
            animator.SetFloat("velocity",Mathf.Abs(Input.GetAxis("Horizontal")));
        }
        else  if (Mathf.Abs(y)>0){
            animator.SetFloat("velocity",Mathf.Abs(Input.GetAxis("Vertical")));
        }
        
        

        //Karakterin dönmesi
        if(Input.GetAxisRaw("Horizontal")==-1){
            transform.rotation=Quaternion.Euler(0f,180f,0f);
        }
        else if(Input.GetAxisRaw("Horizontal")==1){
            transform.rotation=Quaternion.Euler(0f,0f,0f);
        }
    }
}
