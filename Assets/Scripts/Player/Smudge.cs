using UnityEngine;

public class Smudge : MonoBehaviour
{

    [SerializeField]
    private Vector2 lifeTimeRange;
    private float lifeTime;

    private float time;

    private SpriteRenderer sR;

    private void Awake()
    {
        lifeTime = Random.Range(lifeTimeRange.x, lifeTimeRange.y);
        sR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float lifePerc = time / lifeTime;
        sR.color = new Color(1, 1, 1, 1 - lifePerc);
        if (time >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
