using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Rythm rythm;

    private double loopLength;
    private float previousTime = 0;
    public float localTime = 0;

    public virtual void Setup()
    {
        loopLength = rythm.GenerateTimings();
    }

    private void Awake()
    {
        Setup();
    }

    public void UpdateTime(float time)
    {
        previousTime = localTime;
        localTime = (int)((time - (float)rythm.initOffset) * 10000) % (int)(loopLength * 10000) / 10000f;

        if (localTime >= 0 && rythm.Activated(previousTime, localTime))
        {
            Activate();
        }
    }

    public virtual void Activate()
    {
        Debug.Log(this.name + " actived");
    }
}
