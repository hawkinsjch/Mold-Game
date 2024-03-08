using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Min(0)]
    public float walkSpeed;
    public LayerMask layerMask;
    public Rigidbody2D rb;

    public float grappleVelocity = 1;
    private bool grappled = false;
    
    public float maxGroundDistance = 0.1f;

    private Vector2 lastHitPoint;
    private bool lastShotHit = false;
    private Vector2 mousePos;
    private Vector2 lastPlayerPos;

    void Grapple()
    {
        Debug.Log("Shot");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (mousePos - (Vector2)transform.position).normalized, Mathf.Infinity, layerMask);
        lastShotHit = hit;
        if (hit)
        {
            grappled = true;
            lastPlayerPos = transform.position;
            lastHitPoint = hit.point;
            //transform.position = hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        if (lastShotHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHitPoint, 0.15f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(mousePos, 0.15f);
            Gizmos.DrawSphere(lastPlayerPos, 0.15f);
            Gizmos.DrawRay(lastPlayerPos, lastHitPoint - lastPlayerPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, maxGroundDistance + (transform.localScale.y / 2), layerMask))
        {
            float walkDir = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(walkDir * walkSpeed, rb.velocity.y);
        }

        if (grappled)
        {
            rb.velocity = ((lastHitPoint - (Vector2)transform.position).normalized * grappleVelocity);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (!grappled)
            {
                Grapple();
            }
            else
            {
                grappled = false;
            }
        }
    }
}
