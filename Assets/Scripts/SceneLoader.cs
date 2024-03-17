using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "";
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float loadDelay;

    private bool waiting;
    private float waitTime;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (fadeAnimator)
        {
            fadeAnimator.SetTrigger("FadeOut");
            waiting = true;
        }
        else
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
    }

    private void Update()
    {
        if (waiting)
        {
            waitTime += Time.deltaTime;
            if (waitTime >= loadDelay)
            {
                SceneManager.LoadScene(_sceneToLoad);
            }
        }
    }
}