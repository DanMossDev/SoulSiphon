using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceListener : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip bounceEffect;
    GlobalAudio globalAudio;

    void Start() {
        globalAudio = FindObjectOfType<GlobalAudio>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Player") globalAudio.PlaySFX(bounceEffect);
    }
}
