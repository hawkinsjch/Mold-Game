using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatController : MonoBehaviour
{
    public static SplatController instance;

    public GameObject[] splats;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeSplat(RaycastHit2D hit)
    {
        Instantiate(splats[Random.Range(0, splats.Length)], new Vector3(hit.point.x, hit.point.y, 0), transform.rotation);
    }
}
