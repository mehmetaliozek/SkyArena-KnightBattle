using UnityEngine;

public class Stats : MonoBehaviour
{
    // Oyuncunun can barı
    public HealtBar healtBar;
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

        return value;
    }
}
