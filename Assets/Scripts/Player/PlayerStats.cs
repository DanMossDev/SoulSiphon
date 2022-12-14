using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Current Stats
    [Header("Player Variables")]
    public static int score = 0;
    public static string playerElement = "void";
    [Header("Hitpoints")]
    public static int maxHP = 3;
    public static int currentHP = maxHP;
    [Header("Melee")]
    public static int meleeDamage = 1;
    public static float meleeAttackRate = 0.2f;
    [Header("Ranged")]
    public static int projectileDamage = 1;
    public static float projectileSpeed = 20f;
    public static float projectileAttackRate = 0.5f;
    [Header("Movement")]
    public static float moveSpeed = 7f;
    public static float jumpHeight = 15.5f;

    //Maximums for Comparison
    // [Header("Hitpoints")]
    // public static int maxMaxHP = 3;
    // [Header("Melee")]
    // public static int maxMeleeDamage = 1;
    // public static float maxMeleeAttackRate = 0.2f;
    // [Header("Ranged")]
    // public static int maxProjectileDamage = 1;
    // public static float maxProjectileSpeed = 20f;
    // public static float maxProjectileAttackRate = 0.5f;
    // [Header("Movement")]
    // public static float maxMoveSpeed = 7f;
    // public static float maxJumpHeight = 15.5f;


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
