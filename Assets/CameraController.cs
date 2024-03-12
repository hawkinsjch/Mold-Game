using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _ObjectToFollow;
    [SerializeField] private Transform _UpperRightBound;
    [SerializeField] private Transform _LowerLeftBound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       float x = _ObjectToFollow.position.x;
        float y = _ObjectToFollow.position.y;
        if (_UpperRightBound.position.x < x)
        {
            x = _UpperRightBound.position.x;
        }
        else if (_LowerLeftBound.position.x > x)
        {
            x = _LowerLeftBound.position.x;
        }
        if (_UpperRightBound.position.y < y)
        {
            y = _UpperRightBound.position.y;
        }
        else if (_LowerLeftBound.position.y > y)
        {
            y = _LowerLeftBound.position.y;
        }

        gameObject.transform.position = new Vector3(x, y, -10);
    }
}
