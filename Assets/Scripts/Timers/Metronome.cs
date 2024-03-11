using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public float bpm;

    private Timer[] timers;


    private void Awake()
    {
        timers = FindObjectsOfType<Timer>();
    }

    private void Update()
    {
        float deltaBeats = Time.deltaTime * (bpm / 60);

        foreach (Timer t in timers)
        {
            t.UpdateTime(deltaBeats);
        }
    }
}
