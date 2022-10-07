using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [Header("Player Variables")]
    public static int maxHP = 3;
    public static int currentHP = maxHP;
    public static int score = 0;
    public static string playerElement = "void";

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
