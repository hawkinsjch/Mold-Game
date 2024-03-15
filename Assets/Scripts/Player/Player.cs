using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Grapple Shot Settings")]

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    [Min(1)]
    private float grappleMaxDistance = 16;

    [SerializeField]
    [Min(0)]
    private float grappleShootSpeed;
    [SerializeField]
    [Min(0)]
    private float grappleRetractSpeed;

    private float grappleTime = 0;
    private bool grappleHitWall = false;

    private float grappleHitDistance = 0f;
    private float grappleMoveTime;

    private float gravityScale;

    private Vector2 lastHitPoint;
    private Vector2 mousePos;
    private Vector2 lastPlayerPos;


    [Header("Grapple Pull Settings")]

    [SerializeField]
    private float grappleInitVelocity = 1;
    [SerializeField]
    [Min(0)]
    private float grappleAccelerationTime = 0.2f;
    [SerializeField]
    private float grappleMaxVelocity = 1;

    [Header("Health Settings")]

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;

    [Header("Grapple Objects")]

    public GameObject hookPrefab;
    private GameObject hookObj;

    [SerializeField] private GameObject _ropeSprite;
    private GameObject _ropeObject;
    private SpriteRenderer ropeRenderer;

    [Header("Sprites List")]
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite forceSprite;
    [SerializeField] private Sprite smileSprite;

    private GrappleState currentState = GrappleState.None;
    private enum GrappleState
    {
        None,
        Extending,
        Grappled,
        Retracting
    };



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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateRope(Vector2 pos)
    {
        if (_ropeObject)
        {
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
        if (hookObj)
        {
            hookObj.transform.position = pos;
        }
    }

    private void DestroyRope()
    {

        if (hookObj)
        {
            Destroy(hookObj);
        }
        if (_ropeObject)
        {
            Destroy(_ropeObject);
        }
    }

    private void Grapple()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rot = (mousePos - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rot, grappleMaxDistance, layerMask);

        _ropeObject = Instantiate(_ropeSprite, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
        ropeRenderer = _ropeObject.GetComponent<SpriteRenderer>();

        SetupHook(hit);

        currentState = GrappleState.Extending;
        spriteRenderer.sprite = forceSprite;
        if (hit)
        {
            lastHitPoint = hit.point;
            grappleHitWall = true;
        }
        else
        {
            grappleHitWall = false;
            lastHitPoint = (Vector2)transform.position + rot * grappleMaxDistance;
        }
        grappleHitDistance = Vector2.Distance(transform.position, lastHitPoint);
        grappleMoveTime = grappleHitDistance / grappleShootSpeed;
        lastPlayerPos = transform.position;
    }

    private void SetupHook(RaycastHit2D hit)
    {
        hookObj = Instantiate(hookPrefab);
        if (hit)
        {
            hookObj.transform.position = hit.point;
            hookObj.transform.parent = hit.collider.transform;
        }

    }

    private void OnDrawGizmos()
    {
        if (currentState != GrappleState.None)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHitPoint, 0.15f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(mousePos, 0.15f);
            Gizmos.DrawSphere(lastPlayerPos, 0.15f);
            Gizmos.DrawRay(lastPlayerPos, lastHitPoint - lastPlayerPos);
        }
    }

    private void GrappleUpdate()
    {

        // Update Hook
        UpdateRope(lastHitPoint);

        // Grapple Velocity
        Vector2 grappleDir = (lastHitPoint - (Vector2)transform.position).normalized;
        float grappleVelocity = Mathf.Lerp(grappleInitVelocity, grappleMaxVelocity, Mathf.Clamp((grappleTime - grappleMoveTime) / grappleAccelerationTime, 0, 1));

        rb.velocity = (grappleDir * grappleVelocity);
        rb.gravityScale = 0;
    }

    public void Hurt(Vector2 hurtPos, float bounceForce = 0)
    {
        health -= 1;
        currentState = GrappleState.None;
        if (health <= 0)
        {
            spriteRenderer.sprite = deadSprite;
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
        currentState = GrappleState.None;
        grappleTime = 0;
        rb.velocity = Vector2.zero;
        //here.
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
        
        // Grapple Management
        switch (currentState)
        {
            case GrappleState.Extending:
                grappleTime += Time.deltaTime;
                if (grappleTime >= grappleMoveTime)
                {
                    if (grappleHitWall)
                    {
                        currentState = GrappleState.Grappled;
                    }
                    else
                    {
                        StartRetract();
                    }
                }
                else
                {
                    float movePerc = grappleTime / grappleMoveTime;
                    UpdateRope(Vector2.Lerp(transform.position, lastHitPoint, movePerc));
                }
                break;
            case GrappleState.Grappled:
                grappleTime += Time.deltaTime;
                GrappleUpdate();
                break;
            case GrappleState.Retracting:
                float moveAmount = Time.deltaTime * grappleRetractSpeed;
                float distance = Vector2.Distance(hookObj.transform.position, transform.position);
                if (moveAmount >= distance)
                {
                    currentState = GrappleState.None;
                    DestroyRope();
                }
                else
                {
                    UpdateRope(hookObj.transform.position + ((transform.position - hookObj.transform.position).normalized * moveAmount));
                }

                break;
            case GrappleState.None:
                rb.gravityScale = gravityScale;
                grappleTime = 0;
                DestroyRope();
                break;
        }

        // Grapple Shoot
        if (Input.GetMouseButtonDown(0))
        {
            if (currentState == GrappleState.None)
            {
                Grapple();
            }
        }

        if (!Input.GetMouseButton(0) && currentState == GrappleState.Grappled)
        {
            StartRetract();
        }
    }

    private void StartRetract()
    {
        if (hookObj)
        {
            currentState = GrappleState.Retracting;
            spriteRenderer.sprite = smileSprite;
        }
        else
        {
            currentState = GrappleState.None;
        }
    }
}
