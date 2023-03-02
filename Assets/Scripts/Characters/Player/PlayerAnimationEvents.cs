using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public static PlayerAnimationEvents instance;

    public Animator animator;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool canRoll = true;
    [HideInInspector] public bool isDeath = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void AttackStart()
    {
        canMove = false;
        canAttack = false;
    }

    private void AttackEnd()
    {
        canMove = true;
        canAttack = true;
    }

    private void RollStart()
    {
        canAttack = false;
        canRoll = false;
    }

    private void RollEnd()
    {
        canAttack = true;
        canRoll = true;
    }

    private void DeathStart()
    {
        isDeath = true;
        canMove = false;
        canAttack = false;
        canRoll = false;
    }

    private void DeathEnd()
    {
        animator.speed = 0;
    }
}