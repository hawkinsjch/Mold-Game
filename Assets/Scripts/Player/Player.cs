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
    private float grappleInitVelocity = 1;
    [SerializeField]
    [Min(0)]
    private float grappleAccelerationTime = 0.2f;
    [SerializeField]
    private float grappleMaxVelocity = 1;
    [SerializeField]
    [Min(1)]
    private float grappleMaxDistance = 16;
    [SerializeField]
    [Min(0)]
    private float grappleDelayTime = 0.05f;

    [SerializeField]
    [Min(0)]
    private float grappleShootSpeed;
    [SerializeField]
    [Min(0)]
    private float grappleRetractSpeed;


    private bool grappled = false;
    private bool grappleExtending = false;
    private float grappledTime = 0;

    private float grappleHitDistance = 0f;
    private float grappleMoveTime;

    private float gravityScale;

    private Vector2 lastHitPoint;
    private Vector2 mousePos;
    private Vector2 lastPlayerPos;

    [Header("Health Settings")]

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;

    [Header("Grapple Objects")]

    public GameObject hookPrefab;
    private GameObject hookObj;

    [SerializeField]
    private LineRenderer lineRen;

    [SerializeField] private GameObject _ropeSprite;
    private GameObject _ropeObject;
    private SpriteRenderer ropeRenderer;



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

    void UpdateRope(Vector2 pos)
    {
        if (_ropeObject)
        {
            Debug.Log(pos);
            // Rotation Update
            Vector3 look = transform.InverseTransformPoint(new Vector3(pos.x, pos.y, 0));
            float CurAngle = _ropeObject.transform.eulerAngles.z;
            if (CurAngle > 180)
            {
                CurAngle = CurAngle - 360;
            }
            float Angle = Mathf.Atan2(-look.x, look.y) * Mathf.Rad2Deg - 90 - CurAngle;
            _ropeObject.transform.Rotate(0, 0, Angle);

            // Size Update
            ropeRenderer.size = new Vector2(Vector2.Distance(pos, (Vector2)transform.position), 1);
        }
    }

    void Grapple()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rot = (mousePos - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rot, grappleMaxDistance, layerMask);

       

        _ropeObject = Instantiate(_ropeSprite, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
        ropeRenderer = _ropeObject.GetComponent<SpriteRenderer>();


        if (hit)
        {
            UpdateRope(hit.point);

            grappledTime = 0;
            grappled = true;
            lastHitPoint = hit.point;
            SetupHook(hit);
        }
        else
        {
            lastHitPoint = (Vector2)transform.position + rot * grappleMaxDistance;
        }
        grappleHitDistance = Vector2.Distance(transform.position, lastHitPoint);
        grappleMoveTime = grappleHitDistance / grappleShootSpeed;
        lastPlayerPos = transform.position;
        grappleExtending = true;
    }

    void SetupHook(RaycastHit2D hit)
    {
        hookObj = Instantiate(hookPrefab);
        hookObj.transform.position = hit.point;
        hookObj.transform.parent = hit.collider.transform;

    }

    private void OnDrawGizmos()
    {
        if (grappled || grappleExtending)
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
        UpdateRope(lastHitPoint);

        // Update Line
        lineRen.SetPosition(0, lastHitPoint);
        lineRen.SetPosition(1, transform.position);

        // Grapple Velocity
        Vector2 grappleDir = (lastHitPoint - (Vector2)transform.position).normalized;
        float grappleVelocity = grappledTime < grappleDelayTime ? 0 : Mathf.Lerp(grappleInitVelocity, grappleMaxVelocity, Mathf.Clamp((grappledTime - grappleDelayTime) / grappleAccelerationTime, 0, 1));

        rb.velocity = (grappleDir * grappleVelocity);
        rb.gravityScale = 0;
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
        grappled = false;
        grappleExtending = false;
        grappledTime = 0;
        rb.velocity = Vector2.zero;
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
            if (!grappled && !grappleExtending)
            {
                Grapple();
            }
        }
        
        // Grapple Management
        if ((Input.GetMouseButton(0) && grappled) || grappleExtending)
        {
            grappledTime += Time.deltaTime;
            if (grappled && !grappleExtending)
            {
                GrappleUpdate();
            }
            else if (grappleExtending)
            {
                if (grappledTime >= grappleMoveTime)
                {
                    grappleExtending = false;
                }
                else
                {
                    float movePerc = grappledTime / grappleMoveTime;
                    UpdateRope(Vector2.Lerp(transform.position, lastHitPoint, movePerc));
                }
            }
        }
        else
        {
            grappled = false;
            grappleExtending = false;
            rb.gravityScale = gravityScale;
            lineRen.enabled = false;
            grappledTime = 0;
            if (hookObj)
            {
                Destroy(hookObj);
            }
            if (_ropeObject)
            {
                Destroy(_ropeObject);
            }
        }
    }
}
