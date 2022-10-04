using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum Options { 
        Coin
    }
    [Header("Type of Pickup")]
    [SerializeField] Options options; 
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip sfx;
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
            default:
                break;
        }
        globalAudio.PlaySFX(sfx);
        Destroy(this.gameObject);
    }
}
