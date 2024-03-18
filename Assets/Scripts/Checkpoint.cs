using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Collider2D checkPointCollider;
    public Vector2 respawnOffset;

    private void Awake()
    {
        if (!checkPointCollider)
        {
            checkPointCollider = GetComponent<Collider2D>();
        }
        checkPointCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            player.Heal();
            if (player.currentCheckPoint != this)
            {
                GetComponent<Animator>().SetTrigger("Respawn");
            }
            player.currentCheckPoint = this;
        }
    }
}
