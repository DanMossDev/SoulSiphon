using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum Options { 
        Coin,
        Fire,
        Void,
        Holy,
        Item
    }
    [Header("Type of Pickup")]
    [SerializeField] Options options; 
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] sfx;


    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag != "Player") return;
        switch(options.ToString()) {
            case "Coin":
                FindObjectOfType<CoinCounter>().AddCoin();
                break;
            case "Fire":
                PlayerStats.ChangeElement("fire");
                break;
            case "Void":
                PlayerStats.ChangeElement("void");
                break;
            case "Holy":
                PlayerStats.ChangeElement("holy");
                break;
            case "Item":
                GetComponent<StatItem>().UpdateStats();
                break;
            default:
                break;
        }
        GlobalAudio.PlaySFX(sfx);
        Destroy(this.gameObject);
    }
}
