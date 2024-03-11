using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Rythm")]
public class Rythm : ScriptableObject
{
    [SerializeField]
    private double initOffset;
    [SerializeField]
    private double[] activationRythm;

    public double GetOffset()
    {
        return initOffset;
    }

    public (double, int) GetNextTime(int currentIdx)
    {
        int idx = currentIdx + 1 < activationRythm.Length ? currentIdx + 1 : 0;

        return (activationRythm[idx], idx);
    }
}
