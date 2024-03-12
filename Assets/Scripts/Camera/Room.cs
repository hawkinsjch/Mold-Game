using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Vector2Int roomSize;

    private BoxCollider2D col;
    private CameraController cam;

    private Vector2 cameraSize;

    public (Vector2, Vector2) GetBounds()
    {
        Vector2 pos = transform.position;

        float deltaX = Mathf.Max((roomSize.x - cameraSize.x) / 2, 0);
        float deltaY = Mathf.Max((roomSize.y - cameraSize.y) / 2, 0);

        Vector2 deltaVector = new Vector2(deltaX, deltaY);

        Vector2 minBound = (Vector2)transform.position - deltaVector;
        Vector2 maxBound = (Vector2)transform.position + deltaVector;

        return (minBound, maxBound);
    }

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();

        float camScalarSize = Camera.main.orthographicSize;
        cameraSize = new Vector2(camScalarSize / 9 * 16 * 2, camScalarSize * 2);

        col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = roomSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            cam.ChangeRoom(this);
        }
    }
}
