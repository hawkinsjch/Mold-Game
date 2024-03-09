using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private Collider2D trapCollider;
    [SerializeField]
    private bool trapEnabled = true;
    [SerializeField]
    [Min(0)]
    private float bounceForce = 10;

    public void EnableTrap(bool enabled)
    {
        trapEnabled = enabled;
        GetComponent<SpriteRenderer>().enabled = trapEnabled;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (trapEnabled)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                player.Hurt(transform.position, bounceForce);
            }
        }
    }
}
