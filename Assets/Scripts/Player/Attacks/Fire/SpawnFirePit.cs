using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFirePit : MonoBehaviour
{
    [SerializeField] GameObject firePit;
    private void OnCollisionEnter2D(Collision2D other) 
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, LayerMask.GetMask("Ground"));

        if (raycast.collider == null)
        {
            gameObject.layer = LayerMask.NameToLayer("FallingLava");
            GetComponent<Rigidbody2D>().velocity = Vector2.down * 2;
            return;
        }
        Instantiate(firePit, raycast.point, Quaternion.identity);
        Destroy(gameObject);
    }
}
