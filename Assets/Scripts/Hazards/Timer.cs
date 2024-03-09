using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private float rate;
    [SerializeField]
    private float offset;

    private float currentWait = 0;

    public virtual void Setup()
    {
        currentWait = rate - offset;
    }

    private void Awake()
    {
        Setup();
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
