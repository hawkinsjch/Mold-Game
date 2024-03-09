using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private float rate;
    [SerializeField]
    [Min(0)]
    private float offset;

    private float currentWait = 0;

    private void Awake()
    {
        currentWait = -offset;
    }

    private void Update()
    {
        while (currentWait >= rate)
        {
            Activate();
            currentWait = currentWait - rate;
        }

        currentWait += Time.deltaTime;
    }

    public virtual void Activate()
    {
        Debug.Log(this.name + " actived");
    }
}
