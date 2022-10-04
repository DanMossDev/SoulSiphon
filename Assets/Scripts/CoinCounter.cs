using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    GameSession gameSession;
    DisplayCoins displayCoins;
    DisplayScore displayScore;
    int coinCount = 0;
    int thisLevelScore = 0;
    void Start() {
        displayScore = FindObjectOfType<DisplayScore>();
        displayCoins = FindObjectOfType<DisplayCoins>();
        displayCoins.UpdateCount(coinCount);
        displayScore.UpdateScore();
    }
    public void AddCoin()
    {
        coinCount++;
        GameSession.score += 100;
        thisLevelScore += 100;
        displayCoins.UpdateCount(coinCount);
        displayScore.UpdateScore();
    }
    public int CheckCoin()
    {
        return coinCount;
    }

    public void OnDeath()
    {
        GameSession.score -= thisLevelScore;
    }
}
