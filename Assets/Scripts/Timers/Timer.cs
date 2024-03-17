using UnityEngine;

public class Timer : MonoBehaviour
{
    public Rythm rythm;

    [HideInInspector]
    public float localTime = 0;
    [HideInInspector]
    public double nextActTime = 0;

    private int rythmIdx;

    public virtual void Setup()
    {
        localTime = -(float)rythm.GetOffset();
        (nextActTime, rythmIdx) = rythm.GetNextTime(-1);
    }

    private void Awake()
    {
        Setup();
    }

    public void UpdateTime(float deltaBeats)
    {
        localTime += deltaBeats;

        if (localTime >= nextActTime)
        {
            localTime -= (float)nextActTime;
            (nextActTime, rythmIdx) = rythm.GetNextTime(rythmIdx);
            Activate();
        }
    }

    public virtual void Activate()
    {
        Debug.Log(this.name + " actived");
    }
}
