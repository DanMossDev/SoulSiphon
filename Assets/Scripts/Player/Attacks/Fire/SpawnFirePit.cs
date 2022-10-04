using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFirePit : MonoBehaviour
{
    [SerializeField] GameObject firePit;
    private void OnCollisionEnter2D(Collision2D other) 
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        Instantiate(firePit, raycast.point, Quaternion.identity);
    }
}
