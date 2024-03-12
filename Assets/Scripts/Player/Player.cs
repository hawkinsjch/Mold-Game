using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement Settings")]

    [Min(0)]
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float maxGroundDistance = 0.1f;


    [Header("Grapple Settings")]

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    [Min(1)]
    private float maxGrappleDistance = 6;
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

    private float gravityScale;

    private Vector2 lastHitPoint;
    private Vector2 mousePos;
    private Vector2 lastPlayerPos;

    public GameObject hookPrefab;
    private GameObject hookObj;

    [SerializeField]
    private LineRenderer lineRen;

    [Header("Health Settings")]

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;

    [Header("Checkpoint")]
    public Checkpoint currentCheckPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("Player has no RigidBody2D component");
        }
        Respawn();
        gravityScale = rb.gravityScale;
    }

    void Grapple()
    {
        Debug.Log("Shot");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (mousePos - (Vector2)transform.position).normalized, maxGrappleDistance, layerMask);
        if (hit)
        {
            grappledTime = 0;
            grappled = true;
            lastPlayerPos = transform.position;
            lastHitPoint = hit.point;

            lineRen.SetPosition(0, lastHitPoint);
            lineRen.SetPosition(1, transform.position);
            lineRen.enabled = true;

            SetupHook(hit);

            //transform.position = hit.point;
        }
    }

    void SetupHook(RaycastHit2D hit)
    {
        hookObj = Instantiate(hookPrefab);
        hookObj.transform.position = hit.point;
        hookObj.transform.parent = hit.collider.transform;

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
        // Update Hook
        lastHitPoint = hookObj.transform.position;

        // Update Line
        lineRen.SetPosition(0, lastHitPoint);
        lineRen.SetPosition(1, transform.position);

        // Grapple Velocity
        Vector2 grappleDir = (lastHitPoint - (Vector2)transform.position).normalized;
        float grappleVelocity = grappledTime < grappleDelyTime ? 0 : Mathf.Lerp(grappleInitVelocity, grappleMaxVelocity, Mathf.Clamp((grappledTime - grappleDelyTime) / grappleAccelerationTime, 0, 1));

        rb.velocity = (grappleDir * grappleVelocity);
        rb.gravityScale = 0;
        grappledTime += Time.deltaTime;
    }

    public void Hurt(Vector2 hurtPos, float bounceForce = 0)
    {
        health -= 1;
        grappled = false;
        if (health <= 0)
        {
            Respawn();
        }
        else
        {
            rb.velocity = ((Vector2)transform.position - hurtPos).normalized * bounceForce;
        }
    }

    public void Heal()
    {
        health = maxHealth;
    }

    private void Respawn()
    {
        Heal();
        if (currentCheckPoint)
        {
            transform.position = (Vector2)currentCheckPoint.transform.position + currentCheckPoint.respawnOffset;
        }
        else
        {
            transform.position = Vector2.zero;
        }
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
            rb.gravityScale = gravityScale;
            lineRen.enabled = false;
            if (hookObj)
            {
                Destroy(hookObj);
            }
        }
    }
}
