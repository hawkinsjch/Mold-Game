using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Trap
{
    [SerializeField]
    private float speed;
    [SerializeField]
    [Min(0)]
    private float maxLifeSpan;

    [SerializeField]
    private bool followPlayer = false;
    public Vector2 moveDir;

    private float lifeTime;
    private Player player;

    private Rigidbody2D rb;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (!collision.transform.GetComponent<Player>() && !collision.transform.GetComponent<Projectile>())
        {
            Destroy(gameObject);
        }
    }

    private void TrackPlayer()
    {
        if (player)
        {
            moveDir = (player.transform.position - transform.position).normalized;
        }
    }

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        moveDir = moveDir.normalized;

        if (followPlayer)
        {
            player = FindFirstObjectByType<Player>();
        }
    }

    private void Update()
    {
        if (followPlayer)
        {
            TrackPlayer();
        }


        rb.velocity = moveDir * speed;
        lifeTime += Time.deltaTime;

        if (lifeTime >= maxLifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
