using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : SceneLoader
{
    [SerializeField]
    private float waitTime;

    private float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= waitTime)
        {
            LoadScene();
        }
    }
}
