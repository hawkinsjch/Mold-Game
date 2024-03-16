using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public float bpm;
    [SerializeField]
    private float songOffset;

    private Timer[] timers;

    private AudioSource audioSource;

    private float previousAudioTime = 0;

    private void Awake()
    {
        timers = FindObjectsOfType<Timer>();
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning("No Audio Source connected to Metronome");
        }
    }

    private void Update()
    {
        float deltaBeats = 0;
        if (audioSource)
        {
            float audioTime = Mathf.Max(audioSource.time - songOffset, 0);
            deltaBeats = (audioTime - previousAudioTime) * (bpm / 60);
            previousAudioTime = audioTime;
        }
        else
        {
            deltaBeats = Time.deltaTime * (bpm / 60);
        }

        foreach (Timer t in timers)
        {
            t.UpdateTime(deltaBeats);
        }

    }
}
