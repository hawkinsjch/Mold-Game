using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private static GameObject _musicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (_musicPlayer == null)
        {
            _musicPlayer = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
