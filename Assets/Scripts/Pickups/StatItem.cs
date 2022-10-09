using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatItem : MonoBehaviour
{
    [Header("Hitpoints")]
    [SerializeField] int hpIncrease = 0;
    [Header("Melee")]
    [SerializeField] int meleeDamage = 0;
    [SerializeField] float meleeAttackRate = 0;
    [Header("Ranged")]
    [SerializeField] int projectileDamage = 0;
    [SerializeField] float projectileSpeed = 0;
    [SerializeField] float projectileAttackRate = 0;
    [Header("Movement")]
    [SerializeField] float moveSpeed = 0;
    [SerializeField] float jumpHeight = 0;
    [SerializeField] bool restoreHP = false;

    public void UpdateStats()
    {
        //HP
        PlayerStats.maxHP += hpIncrease;
        if (restoreHP) PlayerStats.InitHP();
        //Melee
        PlayerStats.meleeDamage += meleeDamage;
        PlayerStats.meleeAttackRate += meleeAttackRate;
        //Ranged
        PlayerStats.projectileDamage += projectileDamage;
        PlayerStats.projectileSpeed += projectileSpeed;
        PlayerStats.projectileAttackRate += projectileAttackRate;
        //Movement
        PlayerStats.moveSpeed += moveSpeed;
        PlayerStats.jumpHeight += jumpHeight;
    }
}
