using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCollider : MonoBehaviour
{
    ShootBlackHole shootBlackHole;
    

    private void Start() {
        shootBlackHole = FindObjectOfType<ShootBlackHole>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        shootBlackHole.OnReleaseFire();
    }
}
