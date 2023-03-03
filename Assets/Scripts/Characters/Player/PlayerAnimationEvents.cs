using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    // Sahnede bulunan PlayerAnimationEvents scripti
    public static PlayerAnimationEvents instance;

    // Oyuncunun animatoru
    public Animator animator;

    // Oyuncunun yürüme, saldırma, takla atma, hasar alma ve ölme durumlarını tutan değişkenler
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool canRoll = true;
    [HideInInspector] public bool isHurt = false;
    [HideInInspector] public bool isDeath = false;

    // Oyuncunun hasar alma durumunda yanıp sönmesini sağlyan değişkenler
    private float resetCount = 0;
    private float hurtTime = 0.1f;
    private float currentHurtTime;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentHurtTime = hurtTime;
    }

    private void Update()
    {
        // Oyuncu hasar almış ise
        if (isHurt)
        {
            currentHurtTime -= Time.deltaTime;
            if (currentHurtTime <= 0)
            {
                switch (resetCount % 2)
                {
                    case 0:
                        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                        break;
                    case 1:
                        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                        break;
                }
                resetCount++;
                currentHurtTime = hurtTime;
                if (resetCount == 2)
                {
                    isHurt = false;
                    resetCount = 0;
                }
            }
        }
    }

    // Saldırı başlangıcında yürüme ve saldırma kapatılıyor
    private void AttackStart()
    {
        canMove = false;
        canAttack = false;
    }

    // Saldırı başlangıcında kapanan özellikler açılıyor
    private void AttackEnd()
    {
        canMove = true;
        canAttack = true;
    }

    // Takla atma başlangıcında saldırı ve takla atma kapatılıyor kapatılıyor
    private void RollStart()
    {
        canAttack = false;
        canRoll = false;
    }

    // Takla atma başlangıcında kapanan özellikler açılıyor
    private void RollEnd()
    {
        canAttack = true;
        canRoll = true;
    }

    // Ölüm başlangıcında tüm özellikler kapanıyor
    private void DeathStart()
    {
        isDeath = true;
        canMove = false;
        canAttack = false;
        canRoll = false;
    }

    // Ölüm animasyonu tekrar tekrar oynamaması için
    private void DeathEnd()
    {
        animator.speed = 0;
    }
}