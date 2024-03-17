using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "";
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float loadDelay;

    [Header("PauseScreen")]
    [SerializeField] private GameObject PauseScreen;

    private bool waiting;
    private float waitTime;

    private bool _isPaused = true;

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
        if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)
        {
            Pause();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Pause()
    {
        Debug.Log("Pause hit");
        if (PauseScreen != null)
        {
            if (_isPaused)
            {
                //Stops time
                Time.timeScale = 0;
                PauseScreen.SetActive(true);
                _isPaused = false;
            }
            else
            {
                Time.timeScale = 1;
                PauseScreen.SetActive(false);
                _isPaused = true;

            }
        }
       
       
    }
}