using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum Options { 
        Coin,
        Fire,
        Void,
        Holy
    }
    [Header("Type of Pickup")]
    [SerializeField] Options options; 
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] sfx;
    CoinCounter coinCounter;
    GlobalAudio globalAudio;

    void Start() {
        coinCounter = FindObjectOfType<CoinCounter>();
        globalAudio = FindObjectOfType<GlobalAudio>();
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag != "Player") return;
        switch(options.ToString()) {
            case "Coin":
                coinCounter.AddCoin();
                break;
            case "Fire":
                GameSession.ChangeElement("fire");
                break;
            case "Void":
                GameSession.ChangeElement("void");
                break;
            case "Holy":
                GameSession.ChangeElement("holy");
                break;
            default:
                break;
        }
        globalAudio.PlaySFX(sfx);
        Destroy(this.gameObject);
    }
}
