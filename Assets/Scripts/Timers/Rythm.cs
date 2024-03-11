using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Rythm")]
public class Rythm : ScriptableObject
{
    public double initOffset;
    public double[] activationRythm;

    private double[] activationTimes;
    private double length;

    public double GenerateTimings()
    {
        activationTimes = new double[activationRythm.Length];
        double totalTime = 0;
        for (int i = 0; i < activationRythm.Length; i++)
        {
            activationTimes[i] = totalTime;
            totalTime += activationRythm[i];
        }

        length = totalTime;
        return length;
    }

    public double GetNextTiming(float currentTime)
    {
        for (int i = 0; i < activationTimes.Length; i++)
        {
            if (currentTime < activationTimes[i])
            {
                return activationRythm[i];
            }
        }

        return activationRythm[0];
    }

    public bool Activated(float previousTime, float currentTime)
    {
        if (previousTime <= currentTime)
        {
            for (int i = 0; i < activationTimes.Length; i++)
            {
                if (previousTime < activationTimes[i] && currentTime > activationTimes[i])
                {
                    return true;
                }
            }
        }
        else
        {
            // Only happens on idx 0
            return true;
        }

        return false;
    }
}
