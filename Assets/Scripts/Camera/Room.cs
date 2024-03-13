using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Vector2Int roomSize;
    [SerializeField]
    private bool renderRoomGizmo;

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

    private void OnDrawGizmos()
    {
        if (renderRoomGizmo)
        {
            Vector2 min, max;
            min = (Vector2)transform.position - (roomSize / 2);
            max = (Vector2)transform.position + (roomSize / 2);

            Vector2 bL = min;
            Vector2 tL = new Vector2(min.x, max.y);
            Vector2 bR = new Vector2(max.x, min.y);
            Vector2 tR = max;

            Gizmos.color = Color.green;

            Gizmos.DrawRay(bL, tL - bL);
            Gizmos.DrawRay(tL, tR - tL);
            Gizmos.DrawRay(tR, bR - tR);
            Gizmos.DrawRay(bR, bL - bR);
        }
    }

    private void GetCameraInfo()
    {
        cam = Camera.main.GetComponent<CameraController>();

        float camScalarSize = Camera.main.orthographicSize;
        cameraSize = new Vector2(camScalarSize / 9 * 16 * 2, camScalarSize * 2);
    }

    private void Awake()
    {
        GetCameraInfo();

        col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = roomSize - new Vector2(0.05f, 0.05f);
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
