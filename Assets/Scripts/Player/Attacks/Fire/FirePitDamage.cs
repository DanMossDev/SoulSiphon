using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePitDamage : MonoBehaviour
{
    int damagePerSecond = 1;
    int numOfTicks = 3;
    [SerializeField] LayerMask enemyLayer;

    void Start() {
        StartCoroutine(FirePit());
    }

    IEnumerator FirePit()
    {
        while (numOfTicks > 0)
        {
            yield return new WaitForSeconds(1);
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, -2f), 0, enemyLayer);
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyDamageHandler>().TakeDamage(-damagePerSecond, Vector2.zero);
            }
            numOfTicks--;
        }
        Destroy(this.gameObject);
    }

     private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, new Vector2(1, 0.3f));
    }
}
