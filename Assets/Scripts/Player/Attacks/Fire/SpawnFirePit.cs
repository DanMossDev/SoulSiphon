using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFirePit : MonoBehaviour
{
    [SerializeField] GameObject firePit;
    private void OnCollisionEnter2D(Collision2D other) 
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector2.down, 1, LayerMask.GetMask("Ground"));

        if (raycast.collider == null)
        {
            RaycastHit2D rayCastCheck = Physics2D.Raycast(transform.position, Vector2.down, 5, LayerMask.GetMask("Ground"));
            if (rayCastCheck.collider == null) Destroy(gameObject);
            else {
                gameObject.layer = LayerMask.NameToLayer("FallingLava");
                gameObject.transform.localScale = new Vector3(0.25f, 1, 1);
                GetComponent<Rigidbody2D>().velocity = Vector2.down * 10;
                return;
            }
        }
        Instantiate(firePit, raycast.point, Quaternion.identity);
        Destroy(gameObject);
    }
}
