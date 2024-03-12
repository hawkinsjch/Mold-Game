using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform upperBound;
    [SerializeField] private Transform lowerBound;

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Clamp(player.position.x, lowerBound.position.x, upperBound.position.x);
        float y = Mathf.Clamp(player.position.y, lowerBound.position.y, upperBound.position.y);

        gameObject.transform.position = new Vector3(x, y, -10);
    }
}