using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Variables")]
    public static int score = 0;
    public static string playerElement = "void";
    public static int maxHP = 3;
    public static int currentHP = maxHP;
    public static int meleeDamage = 1;
    public static float meleeAttackRate = 0.2f;
    public static int projectileDamage = 1;
    public static float projectileSpeed = 20f;
    public static float projectileAttackRate = 0.5f;

    public delegate void ElementChange();
    public static event ElementChange OnChangeElement;

    public static void ChangeElement(string element)
    {
        playerElement = element;
        OnChangeElement();
    }

    public static void InitHP()
    {
        currentHP = maxHP;
    }
}
