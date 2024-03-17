using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private float transitionTime;

    private Vector2 lastRoomPos;
    private float currentTransitionTime;

    private Transform player;

    private Vector2 upperBound;
    private Vector2 lowerBound;

    private Room room;

    public void ChangeRoom(Room _room)
    {
        if (_room != room)
        {
            if (room)
            {
                lastRoomPos = transform.position;
                currentTransitionTime = 0;
            }
            else
            {
                currentTransitionTime = transitionTime;
            }

            room = _room;
            (lowerBound, upperBound) = room.GetBounds();
        }
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
        if (currentTransitionTime < transitionTime)
        {
            currentTransitionTime += Time.deltaTime;
            float transPerc = Mathf.Clamp(currentTransitionTime / transitionTime, 0, 1);
            x = Mathf.Lerp(lastRoomPos.x, x, transPerc);
            y = Mathf.Lerp(lastRoomPos.y, y, transPerc);
        }

        gameObject.transform.position = new Vector3(x, y, -10);
    }
}