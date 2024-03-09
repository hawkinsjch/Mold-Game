using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapToggler : Timer
{
    [SerializeField]
    private Trap trap;

    public override void Activate()
    {
        trap.Toggle();
    }
}
