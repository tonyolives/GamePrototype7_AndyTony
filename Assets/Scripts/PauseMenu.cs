using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;
    public BackgroundMusicController bkrd;
    [SerializeField] private bool isMainMenu = false;

    private void Start()
    {
        bkrd = FindFirstObjectByType<BackgroundMusicController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameisPaused)
            {
                Resume();
            }
            else
            {
                if (!isMainMenu)
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        bkrd.isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
        bkrd.isPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGunScene()
    {
        Resume();
        SceneManager.LoadScene("TonyGunScene");
    }

    public void LoadAmariScene()
    {
        Resume();
        SceneManager.LoadScene("AmariTestingScene");
    }

    public void LoadAndyScene()
    {
        Resume();
        SceneManager.LoadScene("AndyScene");
    }

    public void LoadForwardScene()
    {
        Resume();
        SceneManager.LoadScene("ForwardScene");
    }

    public void LoadBackTrackScene()
    {
        Resume();
        SceneManager.LoadScene("BacktrackScene");
    }

    public void LoadTutorialScene()
    {
        Resume();
        SceneManager.LoadScene("MainScene");
    }

    public void RestartGame()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

    public void LoadLevelOne()
    {
        Resume();
        bkrd.isPlaying = false;
        SceneManager.LoadScene("Level 1");
        bkrd.isPlaying = false;
    }

    public void LoadLevelTwo()
    {
        Resume();
        bkrd.isPlaying = false;
        SceneManager.LoadScene("Level 2");
        bkrd.isPlaying = false;
    }

    public void LoadLevelThree()
    {
        Resume();
        bkrd.isPlaying = false;
        SceneManager.LoadScene("Level 3");
        bkrd.isPlaying = false;
    }

    public void LoadLevelFour()
    {
        Resume();
        bkrd.isPlaying = false;
        SceneManager.LoadScene("Level 4");
        bkrd.isPlaying = false;
    }

    public void LoadMainMenu()
    {
        Resume();
        bkrd.isPlaying = false;
        SceneManager.LoadScene("Main Menu");
        bkrd.isPlaying = false;
    }

    public void LoadTutorial1()
    {
        Resume();
        bkrd.isPlaying = false;
        SceneManager.LoadScene("Tutorial1");
        bkrd.isPlaying = false;
    }

    public void LoadCredits()
    {
        Resume();
        SceneManager.LoadScene("Credits");
    }

    public void LoadMenuFromCredits()
    {
        Resume();
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenURL(string urlname)
    {
        Application.OpenURL(urlname);
    }
}
