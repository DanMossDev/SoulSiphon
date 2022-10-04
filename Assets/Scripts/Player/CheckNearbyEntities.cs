using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNearbyEntities : MonoBehaviour
{
    ShootBlackHole shootBlackHole;
    
    private void Start() {
        shootBlackHole = FindObjectOfType<ShootBlackHole>();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Enemy") shootBlackHole.UpdateNearbyEntities(other.attachedRigidbody, true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Enemy") shootBlackHole.UpdateNearbyEntities(other.attachedRigidbody, false);
    }
}
