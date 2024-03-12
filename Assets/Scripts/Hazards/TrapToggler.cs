using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapToggler : Timer
{
    [SerializeField]
    private Trap trap;

    public override void Activate()
    {
        bool enabled = trap.Toggle();
        foreach (TogglePart tP in GetComponentsInChildren<TogglePart>())
        {
            tP.SetState(enabled);   
        }
    }
}
