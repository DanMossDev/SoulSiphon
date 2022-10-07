using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingAI : MonoBehaviour
{
    [Space]
    [Header("Pathfinding Settings")]
    [SerializeField] Transform target;
    [SerializeField] float speed = 200f;
    [SerializeField] float nextWaypointDist = 3f;
    [SerializeField] float aggroRange = 5;
    [SerializeField] Transform[] patrolPoints;
    EnemyState state;
    Transform graphics;
    Vector3 destination;
    Path path;
    int currentWaypoint = 0;
    int nextPoint = 0;
    bool reachedDestination = false;

    Seeker seeker;
    Rigidbody2D rigidBody;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        state = GetComponent<EnemyState>();
        rigidBody = GetComponent<Rigidbody2D>();
        graphics = GetComponentInChildren<Transform>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        destination = state.playerInRange ? target.position : patrolPoints[nextPoint].position;
        if (seeker.IsDone()) seeker.StartPath(transform.position, destination, OnPathComplete);
    }

    void OnPathComplete(Path currentPath)
    {
        if (!currentPath.error)
        {
            path = currentPath;
            currentWaypoint = 0;
        }
    }

    void NextPoint()
    {
        if (seeker.IsDone()) {
            if (nextPoint < patrolPoints.Length - 1) nextPoint++;
            else nextPoint = 0;
        }
    }

    void Update() 
    {
        if (!state.playerInRange && Physics2D.OverlapCircleAll(transform.position, aggroRange, LayerMask.GetMask("Player")).Length > 0) state.playerInRange = true;
        if (state.playerInRange && Physics2D.OverlapCircleAll(transform.position, aggroRange * 2, LayerMask.GetMask("Player")).Length == 0) state.playerInRange = false;
    }

    void FixedUpdate()
    {
        print(nextPoint);
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            NextPoint();
            reachedDestination = true;
            return;
        } else reachedDestination = false;

        if (state.isBeingPulled) {state.isBeingPulled = false; return;}

        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        rigidBody.AddForce(direction * speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDist) currentWaypoint++;

        if (transform.position.x - destination.x <= Mathf.Epsilon) graphics.localScale = new Vector3(-1, 1, 1);
        else if (transform.position.x - destination.x >= -Mathf.Epsilon) graphics.localScale = new Vector3(1, 1, 1);
    }

    public void knockBack(Vector2 direction)
    {
        rigidBody.AddForce(direction * 2, ForceMode2D.Impulse);
    }

    public void Die(int direction)
    {
        rigidBody.velocity = new Vector2(direction * 3, 5);
        //animator.SetTrigger("Die");
        GetComponent<DealDamage>().SetDamage(0);
        Destroy(gameObject, 0.2f);
    }
}
