using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;

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
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
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
        SceneManager.LoadScene("Layer 1");
    }

    public void LoadLevelTwo()
    {
        Resume();
        SceneManager.LoadScene("Layer 2");
    }

    public void LoadLevelThree()
    {
        Resume();
        SceneManager.LoadScene("Layer 3");
    }

    public void LoadLevelFour()
    {
        Resume();
        SceneManager.LoadScene("Layer 4");
    }

    public void LoadLevelFive()
    {
        Resume();
        SceneManager.LoadScene("Layer 5");
    }

    public void LoadLevelSix()
    {
        Resume();
        SceneManager.LoadScene("Layer 6");
    }

    public void LoadLevelSeven()
    {
        Resume();
        SceneManager.LoadScene("Layer 7");
    }
}
