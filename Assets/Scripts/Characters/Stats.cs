using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector] public float currentHealth;
    public float attack;
    public float attackRange;
    public float attackRate;
    public float defense;
    public float moveSpeed;

    public float GetStats(int index)
    {
        float value = 0;

        switch (index)
        {
            case 0:
                value = maxHealth;
                break;
            case 1:
                value = attack;
                break;
            case 2:
                value = attackRate;
                break;
            case 3:
                value = defense;
                break;
            case 4:
                value = moveSpeed;
                break;
        }

        return (float)Math.Round((decimal)value, 2);
    }

    public void updateStat()
    {
        float percent = currentHealth / maxHealth;
        maxHealth += maxHealth * 0.15f;
        currentHealth = maxHealth * percent;
        Player.instance.UpdateHealtBar();
        attack += attack * 0.2f;
        attackRate -= attackRate * 0.0225f;
        defense += defense * 0.2f;
        moveSpeed += moveSpeed * 0.025f;
    }
}
