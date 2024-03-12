using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;

    private Vector2 upperBound;
    private Vector2 lowerBound;

    private Room room;

    public void ChangeRoom(Room _room)
    {
        room = _room;
        (lowerBound, upperBound) = room.GetBounds();
    }

    private void Awake()
    {
        player = FindAnyObjectByType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Clamp(player.position.x, lowerBound.x, upperBound.x);
        float y = Mathf.Clamp(player.position.y, lowerBound.y, upperBound.y);

        gameObject.transform.position = new Vector3(x, y, -10);
    }
}