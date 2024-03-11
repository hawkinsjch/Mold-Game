using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Timer
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Vector2 shootDir;

    public override void Setup()
    {
        base.Setup();
        shootDir = shootDir.normalized;
    }

    public override void Activate()
    {
        GameObject projObj = Instantiate(projectile, transform.position, Quaternion.identity);

        projObj.GetComponent<Projectile>().moveDir = shootDir;
    }
}
