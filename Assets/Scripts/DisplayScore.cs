using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    GameSession gameSession;

    void Start() {
 
        UpdateScore();
    }
    public void UpdateScore()
    {
        GetComponent<TextMeshProUGUI>().SetText("Score: " + GameSession.score);
    }
}
