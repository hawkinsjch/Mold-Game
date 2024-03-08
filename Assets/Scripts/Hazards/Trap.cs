using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private Collider2D trapCollider;
    [SerializeField]
    private bool trapEnabled = true;

    public void EnableTrap(bool enabled)
    {
        trapEnabled = enabled;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (trapEnabled)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                player.Hurt();
            }
        }
    }
}
