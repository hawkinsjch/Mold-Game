using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public float bpm;

    private Timer[] timers;

    private float time;

    private void Awake()
    {
        timers = FindObjectsOfType<Timer>();
    }

    private void Update()
    {
        time += Time.deltaTime;

        foreach (Timer t in timers)
        {
            t.UpdateTime(time);
        }
    }
}
