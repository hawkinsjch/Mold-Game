using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : Timer
{
    [SerializeField]
    private Vector2[] points;

    private int nextPoint = -1;
    private int lastPoint = 0;

    [SerializeField]
    private double travelTime = 0;
    [SerializeField]
    private double currentTravelTime = 0;

    public override void Activate()
    {
        lastPoint = nextPoint;
        nextPoint = nextPoint + 1 < points.Length ? nextPoint + 1 : 0;
        travelTime = rythm.GetNextTiming(localTime);
        currentTravelTime = 0;
        print(nextPoint + ": " + travelTime);

        transform.position = points[nextPoint];
    }

    private void DrawRay(int p1, int p2)
    {
        Gizmos.DrawRay(points[p1], points[p2] - points[p1]);
    }

    private void OnDrawGizmos()
    {
        if (points.Length > 1)
        {
            Gizmos.color = Color.grey;
            for (int i = 1; i < points.Length; i++)
            {
                DrawRay(i - 1, i);
            }

            DrawRay(points.Length - 1, 0);
        }
    }

    private void Move()
    {
        currentTravelTime += Time.deltaTime;
        float travelPerc = Mathf.Clamp((float)(currentTravelTime / travelTime), 0, 1);
        Vector2 travelDirection = points[nextPoint] - points[lastPoint];
        transform.position = points[lastPoint] + travelDirection * travelPerc;
    }

    private void Update()
    {
        //Move();
    }
}
