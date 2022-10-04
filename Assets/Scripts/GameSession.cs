using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [Header("Player Variables")]
    public static int maxHP = 3;
    public static int currentHP;
    public static int score = 0;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        currentHP = maxHP;
    }

    public static void InitHP()
    {
        currentHP = maxHP;
    }
}
