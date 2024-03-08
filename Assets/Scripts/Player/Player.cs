using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Min(0)]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private LayerMask layerMask;

    private Rigidbody2D rb;

    [SerializeField]
    private float grappleInitVelocity = 1;
    [SerializeField]
    [Min(0)]
    private float grappleAccelerationTime = 0.2f;
    [SerializeField]
    private float grappleMaxVelocity = 1;
    [SerializeField]
    [Min(0)]
    private float grappleDelyTime = 0.05f;

    private bool grappled = false;
    private float grappledTime = 0;

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;

    [SerializeField]
    private float maxGroundDistance = 0.1f;

    private Vector2 lastHitPoint;
    private Vector2 mousePos;
    private Vector2 lastPlayerPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("Player has no RigidBody2D component");
        }

        Respawn();
    }

    void Grapple()
    {
        Debug.Log("Shot");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (mousePos - (Vector2)transform.position).normalized, Mathf.Infinity, layerMask);
        if (hit)
        {
            grappledTime = 0;
            grappled = true;
            lastPlayerPos = transform.position;
            lastHitPoint = hit.point;
            //transform.position = hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        if (grappled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHitPoint, 0.15f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(mousePos, 0.15f);
            Gizmos.DrawSphere(lastPlayerPos, 0.15f);
            Gizmos.DrawRay(lastPlayerPos, lastHitPoint - lastPlayerPos);
        }
    }

    void GrappleUpdate()
    {
        Vector2 grappleDir = (lastHitPoint - (Vector2)transform.position).normalized;
        float grappleVelocity = grappledTime < grappleDelyTime ? 0 : Mathf.Lerp(grappleInitVelocity, grappleMaxVelocity, Mathf.Clamp((grappledTime - grappleDelyTime) / grappleAccelerationTime, 0, 1));

        rb.velocity = (grappleDir * grappleVelocity);
        rb.gravityScale = 0;
        grappledTime += Time.deltaTime;
    }

    public void Hurt()
    {
        health -= 1;
        grappled = false;
        if (health <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        health = maxHealth;
        transform.position = Vector2.zero;
    }


    // Update is called once per frame
    void Update()
    {
        // Walking
        if (Physics2D.Raycast(transform.position, Vector2.down, maxGroundDistance + (transform.localScale.y / 2), layerMask))
        {
            float walkDir = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(walkDir * walkSpeed, rb.velocity.y);
        }
        
        // Grapple Shoot
        if (Input.GetMouseButtonDown(0)) {
            if (!grappled)
            {
                Grapple();
            }
        }
        
        // Grapple Management
        if (Input.GetMouseButton(0) && grappled)
        {
            GrappleUpdate();
        }
        else
        {
            grappled = false;
            rb.gravityScale = 6;
        }
    }
}
